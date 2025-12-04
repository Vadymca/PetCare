import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { LiqPayService } from '../../../core/services/liq-pay-service.service';
import { FinancialSupportComponent } from '../../../shared/components/financial-support/financial-support.component';
import { IconComponent } from '../../../shared/components/icon.component';

@Component({
  selector: 'app-payment-amount',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    ReactiveFormsModule,
    FinancialSupportComponent,
    IconComponent,
  ],
  templateUrl: './payment-amount.component.html',
  styleUrl: './payment-amount.component.css',
})
export class PaymentAmountComponent implements OnInit {
  backBottomClick() {
    this.router.navigate(['/']);
  }
  liqPay = inject(LiqPayService);
  router = inject(Router);
  initialSum = signal(Number(0));
  initialIsOnce = signal(true);
  ngOnInit(): void {
    const data = this.liqPay.data();
    this.initialIsOnce.set(!data?.isRecurring);
    if (data?.amount && data?.amount > 0) this.initialSum.set(data?.amount);
    // 1. Якщо взагалі немає контексту — викидаємо
    if (!data?.scope) {
      this.liqPay.clear();
      this.router.navigate(['/']);
      return;
    }

    // 2. Особливий випадок: опіка (guardianship)
    if (data.scope === 'guardianship') {
      this.router.navigate(['/payment/details']);
      return;
    }

    // 3. Для всіх інших — залишаємось на сторінці вибору суми
  }
  onSelectionConfirmed(selection: { amount: number; isOnce: boolean }) {
    this.liqPay.update({
      amount: selection.amount,
      isRecurring: !selection.isOnce, // true → false, false → true
    });
    this.next();
  }
  next() {
    const data = this.liqPay.data();

    if (
      !data?.amount ||
      !data.scope ||
      (data.scope === 'guardianship' && !data.entityId) ||
      (data.scope === 'aidRequest' && !data.entityId)
    ) {
      // можна додати toast: "Будь ласка, оберіть суму"
      return;
    }
    this.router.navigate(['/payment/details']);
  }
}
