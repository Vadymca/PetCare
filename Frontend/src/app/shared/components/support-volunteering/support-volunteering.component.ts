import { CommonModule } from '@angular/common';
import { Component, inject, Signal } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from '../../../core/services/auth.service';
import { ModalService } from '../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../buttons/blue/primary-large-button.component';
import { IconComponent } from '../icon.component';

@Component({
  selector: 'app-support-volunteering',

  imports: [
    TranslateModule,
    CommonModule,
    PrimaryLargeButtonComponent,
    IconComponent,
  ],
  templateUrl: './support-volunteering.component.html',
  styleUrl: './support-volunteering.component.css',
})
export class SupportVolunteeringComponent {
  authService = inject(AuthService);
  modalService = inject(ModalService);
  isAuthenticated: Signal<boolean> = this.authService.isLoggedIn;

  volunteeringItems = [
    'WALKING',
    'PHOTO_VIDEO',
    'TELL_ABOUT_US',
    'DELIVERY',
    'COOKING',
  ];
  router = inject(Router);
  onVolunteeringClick() {
    console.log(this.isAuthenticated());
    if (this.isAuthenticated()) {
      //доставити тут знак оклику після тестування
      this.modalService.openModal('welcome');
    } else {
      this.router.navigate(['/support-volunteering-form']);
    }
  }
}
