import { UpperCasePipe } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-email-not-confirmed',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    PrimaryLargeButtonComponent,
    ReactiveFormsModule,
    RouterModule,
    IconComponent,
  ],
  templateUrl: './email-not-confirmed.component.html',
  styleUrl: './email-not-confirmed.component.css',
})
export class EmailNotConfirmedComponent {
  @Output() submitButton = new EventEmitter<void>();
  @Output() resendVerificationEmail = new EventEmitter<void>();

  emitResendVerificationEmail() {
    this.resendVerificationEmail.emit();
  }

  emitSubmitButton() {
    this.submitButton.emit();
  }
}
