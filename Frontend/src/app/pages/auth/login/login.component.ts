import { Component, effect, inject, signal } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from '../../../core/services/auth.service';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, TranslateModule],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  auth = inject(AuthService);
  router = inject(Router);
  route = inject(ActivatedRoute);
  returnUrl = '/';
  showPassword = false;
  errorMessage = signal<string | null>(null);
  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
  });
  constructor() {
    effect(() => {
      const isLoggedIn = this.auth.getIsLoggedIn();
      if (isLoggedIn) {
        this.router.navigate(['/profile']);
      }
    });
    this.route.queryParamMap.subscribe(params => {
      const url = params.get('returnUrl');
      if (url) {
        this.returnUrl = url;
      }
    });
  }
  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;
      console.log('Login attempt:', email, password);
      if (!email || !password) return;
      this.errorMessage.set(null);
      this.auth.login(email, password).subscribe({
        next: res => {
          if (res === '2fa_required') {
            this.router.navigate(['/login-2fa'], {
              queryParams: { returnUrl: this.returnUrl },
            });
          } else {
            // Якщо звичайна авторизація (наприклад, токен), можна тут зробити редірект далі
            this.router.navigate([this.returnUrl]);
          }
        },
        error: err => {
          this.errorMessage.set(err.message || 'AUTH_ERROR');
        },
      });
    } else {
      this.loginForm.markAllAsTouched();
    }
  }
}
