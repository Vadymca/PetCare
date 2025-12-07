import { Component, effect, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { SecondaryLargeButtonComponent } from '../../shared/components/buttons/blue/secondary-large-button.component';
import { ConfirmModalComponent } from '../../shared/components/confirm-modal/confirm-modal.component';
import { IconComponent } from '../../shared/components/icon.component';
import { SocialMediaComponent } from '../../shared/components/social-media/social-media.component';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [
    TranslateModule,
    IconComponent,
    RouterModule,
    SecondaryLargeButtonComponent,
    ReactiveFormsModule,
    SocialMediaComponent,
    ConfirmModalComponent,
  ],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.css',
})
export class FooterComponent {
  onSubscribe() {
    this.showTakeCareModalWindow.set(false);
  }
  fb = new FormBuilder();
  subscriptionForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
  });
  isDisabled = signal(true);
  submitted = signal(false);
  showTakeCareModalWindow = signal(false);
  constructor() {
    effect(() => {
      // Тут беремо значення форми через signal-обгортку
      this.subscriptionForm.valueChanges.subscribe(() => {
        this.isDisabled.set(!this.subscriptionForm.valid);
      });
    });
  }
  onSubmit() {
    this.showTakeCareModalWindow.set(true);
  }
}
