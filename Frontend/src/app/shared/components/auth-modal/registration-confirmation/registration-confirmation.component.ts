import { UpperCasePipe } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

import { ModalState } from '../../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from "../../icon.component";

@Component({
  selector: 'app-registration-confirmation',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    PrimaryLargeButtonComponent,
    ReactiveFormsModule,
    RouterModule,
    IconComponent
],
  templateUrl: './registration-confirmation.component.html',
  styleUrl: './registration-confirmation.component.css',
})
export class RegistrationConfirmationComponent {
  @Output() selectOption = new EventEmitter<ModalState['component']>();
  @Output() resendVerificationEmail = new EventEmitter<void>();

  emitOption(option: ModalState['component']) {
    this.selectOption.emit(option);
  }
  emitResendVerificationEmail() {
    this.resendVerificationEmail.emit();
  }
}
