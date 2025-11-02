import { UpperCasePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { ModalService } from '../../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-change-password-error',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    PrimaryLargeButtonComponent,
    ReactiveFormsModule,
    RouterModule,
    IconComponent,
  ],
  templateUrl: './change-password-error.component.html',
  styleUrl: './change-password-error.component.css',
})
export class ChangePasswordErrorComponent {
  modalService = inject(ModalService);
  router = inject(Router);
  toProfile() {
    this.modalService.closeModal();
    this.router.navigate(['/profile']);
  }
}
