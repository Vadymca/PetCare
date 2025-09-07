import { UpperCasePipe } from '@angular/common';
import { Component, effect, EventEmitter, Output, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { ModalState } from '../../../../core/services/modal.service';
import {
  hasDigitValidator,
  hasLowerCaseValidator,
  hasSpecialCharValidator,
  hasUpperCaseValidator,
  passwordMatchValidator,
} from '../../../validators/password-validators';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    IconComponent,
    PrimaryLargeButtonComponent,
    ReactiveFormsModule,
    RouterModule,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  @Output() selectOption = new EventEmitter<ModalState['component']>();
  @Output() password = new EventEmitter<string>();
  @Output() submitForm = new EventEmitter<void>();

  submitted = signal(false);
  showPassword = signal(false);
  showRepeatPassword = signal(false);

  fb = new FormBuilder();
  registerForm = this.fb.group(
    {
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          hasUpperCaseValidator(),
          hasLowerCaseValidator(),
          hasDigitValidator(),
          hasSpecialCharValidator(),
        ],
      ],
      confirmPassword: ['', [Validators.required]],
      termsAndConditions: [false, Validators.requiredTrue],
    },
    { validators: passwordMatchValidator() }
  );
  isDisabled = signal(true);
  isLoading = signal(false);
  constructor() {
    effect(() => {
      this.registerForm.valueChanges.subscribe(() => {
        this.isDisabled.set(!this.registerForm.valid);
      });
    });
  }
  togglePasswordVisibility() {
    this.showPassword.update(v => !v);
  }

  toggleRepeatPasswordVisibility() {
    this.showRepeatPassword.update(v => !v);
  }
  onSubmit() {
    this.submitted.set(true);
    this.registerForm.markAllAsTouched();
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }
    if (this.registerForm.value.password) {
      this.password.emit(this.registerForm.value.password);
    }

    this.isDisabled.set(true);
    this.isLoading.set(true);
    this.submitForm.emit();
  }
  emitOption(option: ModalState['component']) {
    this.selectOption.emit(option);
  }
}
