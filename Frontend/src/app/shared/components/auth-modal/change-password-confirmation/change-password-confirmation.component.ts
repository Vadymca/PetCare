import { UpperCasePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { ModalService } from '../../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-change-password-confirmation',
  imports: [
    TranslateModule,
    UpperCasePipe,
    PrimaryLargeButtonComponent,
    ReactiveFormsModule,
    RouterModule,
    IconComponent,
  ],
  templateUrl: './change-password-confirmation.component.html',
  styleUrl: './change-password-confirmation.component.css',
})
export class ChangePasswordConfirmationComponent {
  modalService = inject(ModalService);
  router = inject(Router);
  toProfileButton() {
    this.modalService.closeModal();
    this.router.navigate(['/profile']);
  }
}
