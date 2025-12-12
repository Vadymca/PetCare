import { UpperCasePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from '../../../core/services/auth.service';
import { ModalService } from '../../../core/services/modal.service';
import { PrimaryLargeOrangeButtonComponent } from '../../../shared/components/buttons/orange/primary-large-orange-button.component';
import { IconComponent } from '../../../shared/components/icon.component';

@Component({
  selector: 'app-security-settings',
  standalone: true,

  imports: [
    PrimaryLargeOrangeButtonComponent,
    TranslateModule,
    IconComponent,
    UpperCasePipe,
  ],
  templateUrl: './security-settings.component.html',
  styleUrl: './security-settings.component.css',
})
export class SecuritySettingsComponent {
  private auth = inject(AuthService);
  private modal = inject(ModalService);
  status = this.auth.twoFaStatus;
  user = this.auth._currentUser();
  errorMessage = signal<string>('');
  router = inject(Router);

  openSetupTotp() {
    this.errorMessage.set('');
    this.auth.setupTotp().subscribe({
      next: response => {
        this.modal.setQrCodeImage(response.qrCodeImage);
        this.modal.setManualKey(response.manualKey);
        this.modal.openModal('setup-totp');
      },
      error: err => {
        console.error('Setup TOTP error:', err);
        this.errorMessage.set('FAILED_TO_SETUP_TOTP');
        //відобразити модалку помилки
      },
    });
  }
  openSetupSms() {
    this.errorMessage.set('');
    const user = this.auth._currentUser();
    if (!user?.phone) {
      this.errorMessage.set('PHONE_NUMBER_REQUIRED');
      return;
    }
    if (!user.phone.startsWith('+380')) {
      this.errorMessage.set('UKRAINIAN_PHONE_NUMBER_REQUIRED');
      return;
    }
    this.auth.setupSms2fa().subscribe({
      next: () => {
        this.modal.openModal('setup-sms');
      },
      error: err => {
        console.error('Setup SMS 2FA error:', err);
        this.errorMessage.set('FAILED_TO_SETUP_SMS ' + err.error.error);
      },
    });
  }

  openBackupCodes() {
    this.modal.openModal('backup-codes');
  }

  // залишаємо для повного відключення
  openDisableConfirm() {
    this.auth.disableAll2fa();
  }
  toggleTotp() {
    if (!this.status()?.isTwoFactorEnabled) {
      // ? — optional chaining
      this.openSetupTotp();
    } else {
      this.auth.disableTotp().subscribe({
        error: err => console.error('Disable TOTP error', err),
      });
    }
  }

  toggleSms() {
    if (!this.status()?.isSms2FaEnabled) {
      this.openSetupSms();
    } else {
      this.auth.disableSms2fa().subscribe({
        error: err => console.error('Disable SMS 2FA error', err),
      });
    }
  }
  toEditProfile() {
    this.router.navigate(['profile/edit']);
  }
}
