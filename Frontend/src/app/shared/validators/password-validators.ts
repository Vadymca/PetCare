import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

// Валідатор для перевірки вимог до пароля

export function hasUpperCaseValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value || '';
    const hasUpperCase = /[A-Z]/.test(value);
    return hasUpperCase ? null : { hasUpperCase: true };
  };
}
export function hasLowerCaseValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value || '';
    const hasLowerCase = /[a-z]/.test(value);
    return hasLowerCase ? null : { hasLowerCase: true };
  };
}
export function hasDigitValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value || '';
    const hasDigit = /\d/.test(value);
    return hasDigit ? null : { hasDigit: true };
  };
}
export function hasSpecialCharValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value || '';
    const hasSpecialChar = /[\^$*.\\<>?"()|!@#%&/,';:+=\-~]/.test(value);
    return hasSpecialChar ? null : { hasSpecialChar: true };
  };
}

// Валідатор для перевірки збігу паролів
export function passwordMatchValidator(): ValidatorFn {
  return (formGroup: AbstractControl): ValidationErrors | null => {
    const password = formGroup.get('password')?.value;
    const confirmPassword = formGroup.get('confirmPassword')?.value;

    return password === confirmPassword ? null : { passwordMismatch: true };
  };
}
