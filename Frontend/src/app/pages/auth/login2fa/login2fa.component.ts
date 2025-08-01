import { Component, inject, signal } from '@angular/core';
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
  selector: 'app-login2fa',
  standalone: true,
  imports: [ReactiveFormsModule, TranslateModule],
  templateUrl: './login2fa.component.html',
  styleUrl: './login2fa.component.css',
})
export class Login2faComponent {
  auth = inject(AuthService);
  router = inject(Router);
  route = inject(ActivatedRoute);
  returnUrl = '/';
  showError = signal(false);
  errorMessage = signal<string | null>(null);
  codeForm = new FormGroup({
    code: new FormControl('', [
      Validators.required,
      Validators.pattern(/^\d{6}$/), // 6-значний код
    ]),
  });
  constructor() {
    this.route.queryParamMap.subscribe(params => {
      const url = params.get('returnUrl');
      if (url) {
        this.returnUrl = url;
      }
    });
  }
  onSubmit() {
    if (this.codeForm.valid) {
      const code = this.codeForm.value.code;
      if (!code) {
        this.showError.set(true);
        return;
      }
      this.errorMessage.set(null);
      this.auth.verify2fa(code).subscribe({
        next: () => {
          this.router.navigateByUrl(this.returnUrl);
        },
        error: err => {
          this.showError.set(true);
          this.errorMessage.set(err.message || 'INVALID_2FA_CODE');
        },
      });
    } else {
      this.codeForm.markAllAsTouched();
    }
  }
}
