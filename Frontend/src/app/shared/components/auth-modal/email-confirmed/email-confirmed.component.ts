import { UpperCasePipe } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-email-confirmed',
 standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    PrimaryLargeButtonComponent,
    ReactiveFormsModule,
    RouterModule,
    IconComponent,
  ],
  templateUrl: './email-confirmed.component.html',
  styleUrl: './email-confirmed.component.css'
})
export class EmailConfirmedComponent {
 @Output() submitButton = new EventEmitter<void>();

  emitSubmitButton() {
    this.submitButton.emit();
  }
}
