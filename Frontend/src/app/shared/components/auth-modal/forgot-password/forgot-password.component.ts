import { UpperCasePipe } from '@angular/common';
import { Component, effect, EventEmitter, Output, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { ModalState } from '../../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-forgot-password',
  imports: [
    TranslateModule,
    UpperCasePipe,

    IconComponent,
    PrimaryLargeButtonComponent,
    ReactiveFormsModule,
  ],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css',
})
export class ForgotPasswordComponent {
  @Output() selectOption = new EventEmitter<ModalState['component']>();

  @Output() submitButton = new EventEmitter<string>();
  submitted = signal(false);
  fb = new FormBuilder();
  registerForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
  });
  isDisabled = signal(true);
  constructor() {
    effect(() => {
      // Тут беремо значення форми через signal-обгортку
      this.registerForm.valueChanges.subscribe(() => {
        this.isDisabled.set(!this.registerForm.valid);
      });
    });
  }
  onSubmit() {
    this.submitted.set(true);
    this.registerForm.markAllAsTouched();
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    if (this.registerForm.value.email) {
      this.submitButton.emit(this.registerForm.value.email);
    }
  }
  emitOption(option: ModalState['component']) {
    this.selectOption.emit(option);
  }
}
