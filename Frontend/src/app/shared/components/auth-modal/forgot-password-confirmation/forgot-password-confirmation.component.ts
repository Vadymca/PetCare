import { UpperCasePipe } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-forgot-password-confirmation',
  standalone: true,
  imports: [
    IconComponent,
    PrimaryLargeButtonComponent,
    TranslateModule,
    UpperCasePipe,
  ],
  templateUrl: './forgot-password-confirmation.component.html',
  styleUrl: './forgot-password-confirmation.component.css',
})
export class ForgotPasswordConfirmationComponent {
  @Output() submitButton = new EventEmitter<void>();

  emitOption() {
    this.submitButton.emit();
  }
}
