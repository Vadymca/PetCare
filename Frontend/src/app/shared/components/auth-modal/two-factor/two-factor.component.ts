import { UpperCasePipe } from '@angular/common';
import {
  Component,
  effect,
  EventEmitter,
  Input,
  OnDestroy,
  Output,
  Signal,
  signal,
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { TwoFaStatus } from '../../../../core/services/auth.service';
import { ModalState } from '../../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-two-factor',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    IconComponent,
    PrimaryLargeButtonComponent,
    ReactiveFormsModule,
  ],
  templateUrl: './two-factor.component.html',
  styleUrl: './two-factor.component.css',
})
export class TwoFactorComponent implements OnDestroy {
  @Output() selectOption = new EventEmitter<ModalState['component']>(); //+
  @Output() submitButton = new EventEmitter<string>(); //+
  @Output() backupCode = new EventEmitter<string>(); //+
  @Output() resendCode = new EventEmitter<void>(); //+
  @Input() twoFaStatus!: Signal<TwoFaStatus | null>;
  @Input() errorMessage = signal<string>(''); //+
  @Input() loading = signal(false); //+
  @Input() hiddenPhoneNumber = signal<string>(''); //+
  showBackupCodeInput = false;

  isDisabled = signal(true);
  isBackupCodeDisabled = signal(true);
  submitted = signal(false);
  resendTimer = signal(0);
  private intervalId: number | null = null;

  fb = new FormBuilder();
  twoFaForm = this.fb.group({
    code: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
  });
  backupCodeForm = this.fb.group({
    recoveryCode: ['', [Validators.required]],
  });

  constructor() {
    effect(() => {
      this.twoFaForm.valueChanges.subscribe(() => {
        this.isDisabled.set(!this.twoFaForm.valid);
        if (this.loading()) {
          this.isDisabled.set(true);
        }
      });
    });
    effect(() => {
      this.backupCodeForm.valueChanges.subscribe(() => {
        this.isBackupCodeDisabled.set(!this.backupCodeForm.valid);
        if (this.loading()) {
          this.isBackupCodeDisabled.set(true);
        }
      });
    });
  }

  emitResendCode() {
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

  onSubmit() {
    this.showBackupCodeInput = false;
    this.submitted.set(true);
    this.twoFaForm.markAllAsTouched();
    if (this.twoFaForm.invalid) {
      this.twoFaForm.markAllAsTouched();
      return;
    }
    if (this.twoFaForm.value.code) {
      this.submitButton.emit(this.twoFaForm.value.code);
    }
  }
  onSubmitBackupCode() {
    this.submitted.set(true);
    this.backupCodeForm.markAllAsTouched();
    if (this.backupCodeForm.invalid) {
      this.backupCodeForm.markAllAsTouched();
      return;
    }
    if (this.backupCodeForm.value.recoveryCode) {
      this.backupCode.emit(this.backupCodeForm.value.recoveryCode);
    }
  }
  emitOption(option: ModalState['component']) {
    this.selectOption.emit(option);
  }
  useBackupCode() {
    this.selectOption.emit('backup-code-login');
  }
}
