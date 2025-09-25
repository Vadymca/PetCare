import { UpperCasePipe } from '@angular/common';
import {
  Component,
  effect,
  EventEmitter,
  Input,
  Output,
  signal,
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
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
export class TwoFactorComponent {
  @Output() selectOption = new EventEmitter<ModalState['component']>(); //+
  @Output() submitButton = new EventEmitter<string>(); //+
  @Output() backupCode = new EventEmitter<string>(); //+
  @Output() resendCode = new EventEmitter<void>(); //+
  @Input() isTwoFactorEnabled = signal(false); //+
  @Input() isSms2FaEnabled = signal(false); //+
  @Input() errorMessage = signal<string>(''); //+
  @Input() loading = signal(false); //+
  @Input() maskedPhoneNumber = signal<string>(''); //+
  showBackupCodeInput = false;

  isDisabled = signal(true);
  isBackupCodeDisabled = signal(true);
  submitted = signal(false);

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
    this.resendCode.emit();
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
}
