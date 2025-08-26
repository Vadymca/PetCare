import { UpperCasePipe } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-registration-failed',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    PrimaryLargeButtonComponent,
    ReactiveFormsModule,
    RouterModule,
    IconComponent,
  ],
  templateUrl: './registration-failed.component.html',
  styleUrl: './registration-failed.component.css',
})
export class RegistrationFailedComponent {
  @Output() submitButton = new EventEmitter<void>();

  emitSubmitButton() {
    this.submitButton.emit();
  }
}
