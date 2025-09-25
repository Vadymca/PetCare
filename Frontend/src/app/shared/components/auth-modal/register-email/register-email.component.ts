import { CommonModule, LowerCasePipe, UpperCasePipe } from '@angular/common';
import { Component, effect, EventEmitter, Output, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { ModalState } from '../../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-register-email',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    LowerCasePipe,
    CommonModule,
    IconComponent,
    PrimaryLargeButtonComponent,
    ReactiveFormsModule,
  ],
  templateUrl: './register-email.component.html',
  styleUrl: './register-email.component.css',
})
export class RegisterEmailComponent {
  @Output() selectOption = new EventEmitter<ModalState['component']>();
  @Output() email = new EventEmitter<string>();
  @Output() phoneNumber = new EventEmitter<string>();

  submitted = signal(false);
  fb = new FormBuilder();
  registerForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    phoneNumber: ['', [Validators.required]],
  });
  isDisabled = signal(true);
  get emailInvalid() {
    return (
      this.registerForm.controls.email.touched &&
      this.registerForm.controls.email.invalid
    );
  }
  get phoneNumberInvalid() {
    return (
      this.registerForm.controls.phoneNumber.touched &&
      this.registerForm.controls.phoneNumber.invalid
    );
  }
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
      this.email.emit(this.registerForm.value.email);
    }
    if (this.registerForm.value.phoneNumber) {
      this.phoneNumber.emit(this.registerForm.value.phoneNumber);
    }
    this.emitOption('register-name');
  }
  emitOption(option: ModalState['component']) {
    this.selectOption.emit(option);
  }
}
