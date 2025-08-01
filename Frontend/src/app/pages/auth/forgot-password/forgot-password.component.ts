import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [ReactiveFormsModule, TranslateModule, RouterModule],
  templateUrl: './forgot-password.component.html',
})
export class ForgotPasswordComponent {
  auth = inject(AuthService);
  fb = new FormBuilder();
  errorMessage = signal<string | null>(null);
  successMessage = signal<string | null>(null);

  forgotPasswordForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
  });

  onSubmit() {
    if (this.forgotPasswordForm.invalid) {
      this.forgotPasswordForm.markAllAsTouched();
      return;
    }

    this.errorMessage.set(null);
    this.successMessage.set(null);

    const email = this.forgotPasswordForm.value.email!;

    this.auth.forgotPassword(email).subscribe({
      next: () => {
        this.successMessage.set('RESET_LINK_SENT');
      },
      error: err => {
        // Все одно показуємо успішне повідомлення, щоб не "злити" інформацію
        this.successMessage.set('RESET_LINK_SENT');
        console.error('Forgot password error:', err);
      },
    });
  }
}
