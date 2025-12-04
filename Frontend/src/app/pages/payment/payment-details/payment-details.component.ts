import { UpperCasePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { LiqPayService } from '../../../core/services/liq-pay-service.service';
import { SecondaryLargeButtonComponent } from '../../../shared/components/buttons/blue/secondary-large-button.component';
import { IconComponent } from '../../../shared/components/icon.component';

@Component({
  selector: 'app-payment-details',
  standalone: true,
  imports: [
    TranslateModule,
    SecondaryLargeButtonComponent,
    ReactiveFormsModule,
    IconComponent,
    RouterModule,
    UpperCasePipe,
  ],
  templateUrl: './payment-details.component.html',
  styleUrl: './payment-details.component.css',
})
export class PaymentDetailsComponent implements OnInit {
  backBottomClick() {
    this.router.navigate(['/payment/amount']);
  }
  router = inject(Router);
  submitted = signal(false);
  loading = signal(false);
  errorMessage = signal<string | null>(null);
  isDisabled = signal(true);
  fb = new FormBuilder();
  registerForm = this.fb.group({
    fullName: [''],
    phoneNumber: [''],
    email: ['', [Validators.email]],
    termsAndConditions: [false, Validators.requiredTrue],
  });

  liqPay = inject(LiqPayService);
  ngOnInit(): void {
    // 1. Підтягуємо збережені дані з сервісу (якщо людина повернулася назад)
    const data = this.liqPay.data();
    if (data) {
      this.registerForm.patchValue({
        fullName: data.payerName ?? '',
        phoneNumber: data.payerPhone ?? '',
        email: data.payerEmail ?? '',
        termsAndConditions: false, // чекбокс завжди скидаємо — безпека
      });
    }

    // 2. Слухаємо зміни форми → вмикаємо кнопку, коли все валідно
    this.registerForm.valueChanges.subscribe(() => {
      this.isDisabled.set(!this.registerForm.valid || this.loading());
      if (this.errorMessage()) this.errorMessage.set(null);
    });

    // Оновлюємо стан кнопки одразу (на випадок, якщо дані вже були)
    this.isDisabled.set(!this.registerForm.valid);
  }

  get emailInvalid() {
    return (
      this.registerForm.controls.email.touched &&
      this.registerForm.controls.email.invalid
    );
  }
  get nameInvalid() {
    return (
      this.registerForm.controls.fullName.touched &&
      this.registerForm.controls.fullName.invalid
    );
  }
  get phoneNumberInvalid() {
    return (
      this.registerForm.controls.phoneNumber.touched &&
      this.registerForm.controls.phoneNumber.invalid
    );
  }
  onContinueClick() {
    if (this.registerForm.invalid || this.loading()) return;
    this.loading.set(true);
    this.errorMessage.set(null);
    this.isDisabled = signal(true);
    this.submitted.set(true);
    const { fullName, phoneNumber, email } = this.registerForm.value;
    this.liqPay.update({
      payerName: fullName?.trim() || undefined,
      payerPhone: phoneNumber?.trim() || undefined,
      payerEmail: email?.trim() || undefined,
    });
    const data = this.liqPay.data();
    if (!data || !data.scope) {
      this.errorMessage.set('PAYMENT_NOT_SELECTED');
      return;
    }
    if (data.scope !== 'guardianship' && (!data.amount || data.amount === 0)) {
      this.errorMessage.set('PAYMENT_NOT_SELECTED');
      return;
    }
    if ((!data.entityId || data.entityId === '') && data.scope !== 'global') {
      this.errorMessage.set('PAYMENT_NOT_SELECTED');
      return;
    }

    this.liqPay.proceed().subscribe({
      next: response => {
        const form = document.createElement('form');
        form.method = 'POST';
        form.action = response.gatewayUrl; // → https://www.liqpay.ua/api/3/checkout
        form.style.display = 'none';

        const dataInput = document.createElement('input');
        dataInput.name = 'data';
        dataInput.value = response.data;
        form.appendChild(dataInput);

        const signatureInput = document.createElement('input');
        signatureInput.name = 'signature';
        signatureInput.value = response.signature;
        form.appendChild(signatureInput);

        document.body.appendChild(form);
        form.submit(); // Відкриває LiqPay у тій самій вкладці — ідеально!
      },
      error: err => {
        console.error('LiqPay error:', err);
        this.errorMessage.set(err?.error?.message || 'PAYMENT_ERROR');
        this.loading.set(false);
        this.isDisabled.set(false);
      },
    });
  }
}
