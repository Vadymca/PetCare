import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, effect, inject, PLATFORM_ID, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { filter } from 'rxjs';
import { PrimaryLargeButtonComponent } from '../../shared/components/buttons/blue/primary-large-button.component';
import { IconComponent } from '../../shared/components/icon.component';

@Component({
  selector: 'app-feedback-form',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    ReactiveFormsModule,

    IconComponent,
    PrimaryLargeButtonComponent,
  ],
  templateUrl: './feedback-form.component.html',
  styleUrl: './feedback-form.component.css',
})
export class FeedbackFormComponent {
  router = inject(Router);
  submitted = signal(false);

  fb = new FormBuilder();
  feedbackForm = this.fb.group({
    name: ['', [Validators.required]],
    phoneNumber: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    topic: ['', [Validators.required]],
    message: ['', [Validators.required]],
  });
  isDisabled = signal(true);
  platformId = inject(PLATFORM_ID);
  constructor() {
    effect(() => {
      // Тут беремо значення форми через signal-обгортку
      this.feedbackForm.valueChanges.subscribe(() => {
        this.isDisabled.set(!this.feedbackForm.valid);
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

  get nameInvalid() {
    return (
      this.feedbackForm.controls.name.touched &&
      this.feedbackForm.controls.name.invalid
    );
  }
  get emailInvalid() {
    return (
      this.feedbackForm.controls.email.touched &&
      this.feedbackForm.controls.email.invalid
    );
  }
  get phoneNumberInvalid() {
    return (
      this.feedbackForm.controls.phoneNumber.touched &&
      this.feedbackForm.controls.phoneNumber.invalid
    );
  }
  get topicInvalid() {
    return (
      this.feedbackForm.controls.topic.touched &&
      this.feedbackForm.controls.topic.invalid
    );
  }
  get messageInvalid() {
    return (
      this.feedbackForm.controls.message.touched &&
      this.feedbackForm.controls.message.invalid
    );
  }

  backBottomClick() {
    this.router.navigate(['contacts']);
  }
  onSubmit() {
    this.submitted.set(true);
    this.feedbackForm.markAllAsTouched();
    if (this.feedbackForm.invalid) {
      this.feedbackForm.markAllAsTouched();
      return;
    }

    // Тут виклик API для збереження даних
  }
  refillForm() {
    this.submitted.set(false);
    this.feedbackForm.reset();
  }
}
