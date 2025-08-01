import { Component, inject, signal } from '@angular/core';

import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { User } from '../../../core/models/user';
import { AuthService } from '../../../core/services/auth.service';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, TranslateModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  auth = inject(AuthService);
  router = inject(Router);
  fb = new FormBuilder();
  showPassword = signal(false);
  showRepeatPassword = signal(false);
  submitted = signal(false);
  errorMessage = signal<string | null>(null);

  registerForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
    confirmPassword: ['', Validators.required],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    phone: [
      '',
      [
        Validators.required,
        Validators.pattern(/^(?=(?:.*\d){5,})[\d+\-\s()]+$/),
      ],
    ],
  });
  togglePasswordVisibility() {
    this.showPassword.update(v => !v);
  }

  toggleRepeatPasswordVisibility() {
    this.showRepeatPassword.update(v => !v);
  }
  onSubmit() {
    this.submitted.set(true);

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const { password, confirmPassword, ...rest } = this.registerForm.value;

    if (password !== confirmPassword || !password) {
      this.errorMessage.set('PASSWORDS_DO_NOT_MATCH');
      return;
    }

    this.errorMessage.set(null);

    // Підготовка об'єкта користувача
    const user: Partial<User> = {
      password,
      email: rest.email ?? undefined,
      firstName: rest.firstName ?? undefined,
      lastName: rest.lastName ?? undefined,
      phone: rest.phone ?? undefined,
    };

    this.auth.register(user).subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: err => {
        console.error('Registration error:', err);
        if (err.error?.message) {
          this.errorMessage.set(err.error.message);
        } else {
          this.errorMessage.set('REGISTRATION_FAILED');
        }
      },
    });
  }
}
