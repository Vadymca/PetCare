import { UpperCasePipe } from '@angular/common';
import { Component, effect, EventEmitter, Output, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
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
  selector: 'app-reset-password',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    IconComponent,
    PrimaryLargeButtonComponent,
    ReactiveFormsModule,
    RouterModule,
  ],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css',
})
export class ResetPasswordComponent {
  @Output() submitForm = new EventEmitter<string>();

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
    },
    { validators: passwordMatchValidator() }
  );
  isDisabled = signal(true);
  isLoading = signal(false);
  constructor() {
    effect(() => {
      this.registerForm.valueChanges.subscribe(() => {
        this.isDisabled.set(!this.registerForm.valid);
        if (this.isLoading()) {
          this.isDisabled.set(true);
        }
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
    this.registerForm.markAllAsTouched();
    if (this.registerForm.invalid) {
      return;
    }
    if (!this.registerForm.value.password) {
      return;
    }

    this.isDisabled.set(true);
    this.isLoading.set(true);
    this.submitForm.emit(this.registerForm.value.password);
  }
}
