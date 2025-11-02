import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnDestroy,
  OnInit,
  Output,
  signal,
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from '../../../../core/services/auth.service';
import { PrimaryLargeOrangeButtonComponent } from '../../buttons/orange/primary-large-orange-button.component';

@Component({
  selector: 'app-setup-sms',
  standalone: true,
  imports: [
    PrimaryLargeOrangeButtonComponent,
    TranslateModule,
    ReactiveFormsModule,
  ],
  templateUrl: './setup-sms.component.html',
  styleUrl: './setup-sms.component.css',
})
export class SetupSmsComponent implements OnInit, OnDestroy {
  hiddenPhoneNumber = ''; // маскований телефон
  @Input() loading = signal(false);
  @Input() errorMessage = signal('');

  @Output() submitCode = new EventEmitter<string>();
  @Output() resendCode = new EventEmitter<void>();
  resendTimer = signal(0);
  private intervalId: number | null = null;
  authService = inject(AuthService);
  fb = new FormBuilder();
  smsForm = this.fb.group({
    code: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
  });

  isDisabled = signal(true);

  ngOnInit(): void {
    this.smsForm.valueChanges.subscribe(() => {
      this.isDisabled.set(!this.smsForm.valid || this.loading());
    });

    this.hiddenPhoneNumber = this.authService.getCurrentUser()?.phone || '';
  }

  onSubmit() {
    if (this.smsForm.valid && this.smsForm.value.code) {
      this.submitCode.emit(this.smsForm.value.code);
    } else {
      this.smsForm.markAllAsTouched();
    }
  }

  onResend() {
    if (this.resendTimer() > 0 || this.loading()) return; // поки таймер працює, не дозволяємо клік
    this.resendCode.emit();
    this.startResendTimer();
  }
  private startResendTimer() {
    this.resendTimer.set(30); // 30 секунд
    this.intervalId = window.setInterval(() => {
      this.resendTimer.update(v => v - 1);
      if (this.resendTimer() <= 0) {
        if (this.intervalId !== null) {
          clearInterval(this.intervalId);
          this.intervalId = null; // обнуляємо після очищення
        }
      }
    }, 1000);
  }
  ngOnDestroy(): void {
    if (this.intervalId !== null) {
      clearInterval(this.intervalId);
      this.intervalId = null;
    }
  }
}
