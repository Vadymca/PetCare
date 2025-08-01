import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

import { signal } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  imports: [ReactiveFormsModule, TranslateModule, RouterModule],
  standalone: true,
  styleUrls: ['./reset-password.component.css'],
})
export class ResetPasswordComponent {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private auth = inject(AuthService);

  errorMessage = signal<string | null>(null);
  successMessage = signal<string | null>(null);
  submitted = signal(false);
  token = signal<string | null>(null);

  showPassword = signal(false);
  showRepeatPassword = signal(false);

  togglePasswordVisibility() {
    this.showPassword.update(v => !v);
  }

  toggleRepeatPasswordVisibility() {
    this.showRepeatPassword.update(v => !v);
  }
  constructor() {
    //?token=... якщо так
    // this.route.queryParamMap.subscribe(params => {
    //   const token = params.get('token');
    //   if (token) {
    //     this.token.set(token);
    //   } else {
    //     this.router.navigate(['/login']);
    //   }
    // });
    //якщо /token
    this.route.paramMap.subscribe(params => {
      const token = params.get('token');
      if (token) {
        this.token.set(token);
      } else {
        this.router.navigate(['/login']);
      }
    });
  }

  resetPasswordForm = this.fb.group({
    password: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', [Validators.required]],
  });

  get passwordMismatch(): boolean {
    return (
      this.resetPasswordForm.value.password !==
        this.resetPasswordForm.value.confirmPassword &&
      this.resetPasswordForm.touched
    );
  }

  onSubmit() {
    this.submitted.set(true);

    this.errorMessage.set(null);
    if (this.resetPasswordForm.invalid) {
      this.resetPasswordForm.markAllAsTouched();
      return;
    }

    const { password, confirmPassword } = this.resetPasswordForm.value;

    if (password !== confirmPassword) {
      this.errorMessage.set('PASSWORDS_DO_NOT_MATCH');
      return;
    }
    const token = this.token()?.toString();

    if (token) {
      this.auth.resetPassword(token, password!).subscribe({
        next: () => {
          this.successMessage.set('PASSWORD_RESET_SUCCESS');
          setTimeout(() => this.router.navigate(['/login']), 2000);
        },
        error: err => {
          console.error('Reset password error:', err);
          this.errorMessage.set(err?.error?.message || 'RESET_PASSWORD_FAILED');
        },
      });
    }
  }
}
