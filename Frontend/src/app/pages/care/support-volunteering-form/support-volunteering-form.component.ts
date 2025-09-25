import {
  CommonModule,
  isPlatformBrowser,
  UpperCasePipe,
} from '@angular/common';
import { Component, effect, inject, PLATFORM_ID, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { filter } from 'rxjs';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { IconComponent } from '../../../shared/components/icon.component';

@Component({
  selector: 'app-support-volunteering-form',
  standalone: true,
  imports: [
    TranslateModule,
    IconComponent,
    UpperCasePipe,
    CommonModule,
    ReactiveFormsModule,
    PrimaryLargeButtonComponent,
    RouterModule,
  ],
  templateUrl: './support-volunteering-form.component.html',
  styleUrl: './support-volunteering-form.component.css',
})
export class SupportVolunteeringFormComponent {
  router = inject(Router);
  backButtonClick() {
    this.router.navigate(['support']);
  }
  submitted = signal(false);
  fb = new FormBuilder();
  registerForm = this.fb.group({
    name: ['', [Validators.required]],
    phoneNumber: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    comment: [''],
    sm1: [''],
    sm2: [''],
    sm3: [''],
    type1: [false],
    type2: [false],
    type3: [false],
    type4: [false],
    termsAndConditions: [false, Validators.requiredTrue],
    careRules: [false, Validators.requiredTrue],
  });
  isDisabled = signal(true);
  get nameInvalid() {
    return (
      this.registerForm.controls.name.touched &&
      this.registerForm.controls.name.invalid
    );
  }
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
  platformId = inject(PLATFORM_ID);
  constructor() {
    effect(() => {
      // Тут беремо значення форми через signal-обгортку
      this.registerForm.valueChanges.subscribe(() => {
        this.isDisabled.set(!this.registerForm.valid);
      });
    });
    if (isPlatformBrowser(this.platformId)) {
      this.router.events
        .pipe(filter(event => event instanceof NavigationEnd))
        .subscribe(() => {
          window.scrollTo({ top: 0, behavior: 'auto' });
        });
    }
  }

  onSubmit() {
    this.submitted.set(true);
    this.registerForm.markAllAsTouched();
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    // Тут виклик API для збереження даних
    this.router.navigate(['volunteer-application-confirmation']);
  }
}
