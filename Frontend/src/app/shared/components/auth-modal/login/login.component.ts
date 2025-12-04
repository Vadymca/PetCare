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
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { ModalState } from '../../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    IconComponent,
    PrimaryLargeButtonComponent,
    ReactiveFormsModule,
    RouterModule,


  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  @Output() selectOption = new EventEmitter<ModalState['component']>();
  @Output() email = new EventEmitter<string>();
  @Output() password = new EventEmitter<string>();
  @Output() submitForm = new EventEmitter<void>();
  @Input() errorMessage = signal<string>('');
  @Input() loading = signal(false);

  submitted = signal(false);
  showPassword = signal(false);
  isDisabled = signal(true);
  isLoading = signal(false);

  fb = new FormBuilder();
  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });
  constructor() {
    effect(() => {
      this.loginForm.valueChanges.subscribe(() => {
        this.isDisabled.set(!this.loginForm.valid);
        if (this.loading()) {
          this.isDisabled.set(true);
        }
      });
    });
  }
  togglePasswordVisibility() {
    this.showPassword.update(v => !v);
  }
  emitOption(option: ModalState['component']) {
    this.selectOption.emit(option);
  }

  onSubmit() {
    this.isLoading.set(true);
    this.submitted.set(true);
    this.loginForm.markAllAsTouched();
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }
    if (this.loginForm.value.email) {
      this.email.emit(this.loginForm.value.email);
    }
    if (this.loginForm.value.password) {
      this.password.emit(this.loginForm.value.password);
    }
    this.submitForm.emit();
  }
}
