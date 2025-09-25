import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { IconComponent } from '../icon.component';
import { SecondaryLargeOrangeButtonWithBorderComponent } from "../buttons/orange/secondary-large-orange-button-with-border.component";
import { PrimaryLargeOrangeButtonWithBorderComponent } from "../buttons/orange/primary-large-orange-button-with-border.component";

@Component({
  selector: 'app-confirm-modal',
  standalone: true,
  imports: [TranslateModule, IconComponent, SecondaryLargeOrangeButtonWithBorderComponent, PrimaryLargeOrangeButtonWithBorderComponent],
  templateUrl: './confirm-modal.component.html',
  styleUrl: './confirm-modal.component.css',
})
export class ConfirmModalComponent {
  @Input() text = 'Are you sure?';
  @Input() titleSubmit = 'SUBMIT';
  @Input() titleCancel = 'CANCEL';
  @Output() confirmAction = new EventEmitter<boolean>();
  confirmLogout(arg0: boolean) {
    this.confirmAction.emit(arg0);
  }
}
