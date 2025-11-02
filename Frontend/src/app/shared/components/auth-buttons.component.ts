import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { ModalService } from '../../core/services/modal.service';
import { PrimarySmallOrangeButtonComponent } from './buttons/orange/primary-small-orange-button.component';

@Component({
  selector: 'app-auth-buttons',
  standalone: true,
  imports: [TranslateModule, PrimarySmallOrangeButtonComponent],
  template: `
    <app-primary-small-orange-button
      [iconName]="'userRound'"
      [buttonTitle]="'LOGIN'"
      (click)="goToLogin()"
    ></app-primary-small-orange-button>
  `,
})
export class AuthButtonsComponent {
  router = inject(Router);
  modalService = inject(ModalService);

  goToLogin() {
    this.modalService.openModal('welcome');
    // this.router.navigate(['/login']);
  }
}
