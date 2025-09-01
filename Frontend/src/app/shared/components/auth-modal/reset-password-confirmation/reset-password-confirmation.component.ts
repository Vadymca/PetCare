import { UpperCasePipe } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-reset-password-confirmation',
  imports: [
    TranslateModule,
    UpperCasePipe,
    PrimaryLargeButtonComponent,
    ReactiveFormsModule,
    RouterModule,
    IconComponent,
  ],
  templateUrl: './reset-password-confirmation.component.html',
  styleUrl: './reset-password-confirmation.component.css',
})
export class ResetPasswordConfirmationComponent {
  @Output() submitButton = new EventEmitter<void>();
  emitSubmitButton() {
    this.submitButton.emit();
  }
}
