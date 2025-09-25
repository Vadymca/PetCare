import { UpperCasePipe } from '@angular/common';
import { Component, effect, EventEmitter, Output, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { ModalState } from '../../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-register-name',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,

    IconComponent,
    PrimaryLargeButtonComponent,
    ReactiveFormsModule,
  ],
  templateUrl: './register-name.component.html',
  styleUrl: './register-name.component.css',
})
export class RegisterNameComponent {
  @Output() selectOption = new EventEmitter<ModalState['component']>();
  @Output() firstName = new EventEmitter<string>();
  @Output() lastName = new EventEmitter<string>();
  @Output() postalCode = new EventEmitter<string>();
  errorMessage = signal<string | null>(null);
  submitted = signal(false);
  fb = new FormBuilder();
  registerForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    postalCode: ['', Validators.required],
  });
  isDisabled = signal(true);
  constructor() {
    effect(() => {
      this.registerForm.valueChanges.subscribe(() => {
        this.isDisabled.set(!this.registerForm.valid);
        // Оновлюємо помилки
        this.updateErrors();
      });
    });
  }
  updateErrors() {
    const errors: string[] = [];
    const controls = this.registerForm.controls;

    if (controls.firstName.touched && controls.firstName.invalid) {
      errors.push('FIRST_NAME_REQUIRED');
    }
    if (controls.lastName.touched && controls.lastName.invalid) {
      errors.push('LAST_NAME_REQUIRED');
    }
    if (controls.postalCode.touched && controls.postalCode.invalid) {
      errors.push('ZIP_CODE_REQUIRED');
    }

    // Оновлюємо помилки
    this.errorMessage.set(errors.join('. '));
  }
  onSubmit() {
    this.submitted.set(true);

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }
    this.errorMessage.set(null);
    if (this.registerForm.value.firstName)
      this.firstName.emit(this.registerForm.value.firstName);
    if (this.registerForm.value.lastName)
      this.lastName.emit(this.registerForm.value.lastName);
    if (this.registerForm.value.postalCode)
      this.postalCode.emit(this.registerForm.value.postalCode);

    this.emitOption('register');
  }
  emitOption(option: ModalState['component']) {
    this.selectOption.emit(option);
  }
}
