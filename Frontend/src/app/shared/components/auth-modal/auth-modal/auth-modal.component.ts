import { Component, effect, inject, signal } from '@angular/core';

import { AuthService, AuthStep } from '../../../../core/services/auth.service';
import {
  ModalService,
  ModalState,
} from '../../../../core/services/modal.service';
import { IconComponent } from '../../icon.component';
import { BackupCodeLoginComponent } from '../backup-code-login/backup-code-login.component';
import { BackupCodesComponent } from '../backup-codes/backup-codes.component';
import { ChangePasswordConfirmationComponent } from '../change-password-confirmation/change-password-confirmation.component';
import { ChangePasswordErrorComponent } from '../change-password-error/change-password-error.component';
import { ChangePasswordComponent } from '../change-password/change-password.component';
import { EmailConfirmedComponent } from '../email-confirmed/email-confirmed.component';
import { EmailNotConfirmedComponent } from '../email-not-confirmed/email-not-confirmed.component';
import { ExistingEmailErrorComponent } from '../existing-email-error/existing-email-error.component';
import { ForgotPasswordConfirmationComponent } from '../forgot-password-confirmation/forgot-password-confirmation.component';
import { ForgotPasswordComponent } from '../forgot-password/forgot-password.component';
import { LiveDonationCollectionComponent } from '../live-donation-collection/live-donation-collection.component';
import { LoginComponent } from '../login/login.component';
import { RegisterEmailComponent } from '../register-email/register-email.component';
import { RegisterNameComponent } from '../register-name/register-name.component';
import { RegisterComponent } from '../register/register.component';
import { RegistrationConfirmationComponent } from '../registration-confirmation/registration-confirmation.component';
import { RegistrationFailedComponent } from '../registration-failed/registration-failed.component';
import { ResetPasswordConfirmationComponent } from '../reset-password-confirmation/reset-password-confirmation.component';
import { ResetPasswordErrorComponent } from '../reset-password-error/reset-password-error.component';
import { ResetPasswordComponent } from '../reset-password/reset-password.component';
import { SendEmailErrorComponent } from '../send-email-error/send-email-error.component';
import { SetupSmsComponent } from '../setup-sms/setup-sms.component';
import { SetupTotpComponent } from '../setup-totp/setup-totp.component';
import { TwoFactorComponent } from '../two-factor/two-factor.component';
import { WelcomeComponent } from '../welcome/welcome.component';
@Component({
  selector: 'app-auth-modal',
  standalone: true,
  imports: [
    WelcomeComponent,
    LoginComponent,
    RegisterComponent,
    TwoFactorComponent,
    IconComponent,
    RegisterEmailComponent,
    RegisterNameComponent,
    RegistrationConfirmationComponent,
    SendEmailErrorComponent,
    RegistrationFailedComponent,
    ExistingEmailErrorComponent,
    EmailConfirmedComponent,
    EmailNotConfirmedComponent,
    ForgotPasswordComponent,
    ForgotPasswordConfirmationComponent,
    ResetPasswordComponent,
    ResetPasswordConfirmationComponent,
    ResetPasswordErrorComponent,
    LiveDonationCollectionComponent,
    ChangePasswordComponent,
    ChangePasswordConfirmationComponent,
    ChangePasswordErrorComponent,
    SetupTotpComponent,
    SetupSmsComponent,
    BackupCodesComponent,
    BackupCodeLoginComponent,
  ],
  template: `
    @if (modalState().isOpen) {
      <div
        class="overflow-x-hidden fixed inset-0 z-[99999] bg-secondary-neutral-mineShaft bg-opacity-50 flex items-center justify-center p-2"
      >
        <div
          class="bg-secondary-neutral-white text-secondary-neutral-mineShaft p-4 rounded-[40px] shadow-2xl
           w-full max-w-[400px] sm:max-w-[560px] min-w-[320px] max-h-[90vh] overflow-auto mx-2"
        >
          <div class="flex justify-end">
            <app-icon
              [name]="'close'"
              class="cursor-pointer hover:text-primary-orange"
              (click)="closeModal()"
            ></app-icon>
          </div>
          <div class="flex-1">
            <!-- Динамічне відображення компонента -->
            @switch (modalState().component) {
              @case ('welcome') {
                <app-welcome
                  (selectOption)="handleOption($event)"
                ></app-welcome>
              }
              @case ('login') {
                <app-login
                  (email)="handleEmail($event)"
                  (password)="handlePassword($event)"
                  (submitForm)="handleSubmitLoginForm()"
                  (selectOption)="handleOption($event)"
                  [errorMessage]="errorMessage"
                  [loading]="isLoading"
                ></app-login>
              }
              @case ('register-email') {
                <app-register-email
                  (selectOption)="handleOption($event)"
                  (email)="handleEmail($event)"
                  (phoneNumber)="handlePhoneNumber($event)"
                ></app-register-email>
              }
              @case ('register-name') {
                <app-register-name
                  (selectOption)="handleOption($event)"
                  (firstName)="handleFirstName($event)"
                  (lastName)="handleLastName($event)"
                  (postalCode)="handleZipCode($event)"
                ></app-register-name>
              }
              @case ('register') {
                <app-register
                  (selectOption)="handleOption($event)"
                  (password)="handlePassword($event)"
                  (submitForm)="handleSubmitRegistrationForm()"
                ></app-register>
              }
              @case ('registration-confirmation') {
                <app-registration-confirmation
                  (selectOption)="handleOption($event)"
                  (resendVerificationEmail)="handleResendVerificationEmail()"
                ></app-registration-confirmation>
              }
              @case ('registration-failed') {
                <app-registration-failed
                  (submitButton)="handleOption('welcome')"
                ></app-registration-failed>
              }
              @case ('existing-email-error') {
                <app-existing-email-error
                  (submitButton)="handleOption('login')"
                ></app-existing-email-error>
              }
              @case ('send-email-error') {
                <app-send-email-error
                  (submitButton)="handleOption('welcome')"
                ></app-send-email-error>
              }
              @case ('email-confirmed') {
                <app-email-confirmed
                  (submitButton)="handleOption('login')"
                ></app-email-confirmed>
              }
              @case ('email-not-confirmed') {
                <app-email-not-confirmed
                  (submitButton)="handleOption('login')"
                  (resendVerificationEmail)="handleResendVerificationEmail()"
                ></app-email-not-confirmed>
              }

              @case ('two-factor') {
                <app-two-factor
                  [errorMessage]="errorMessage"
                  [loading]="isLoading"
                  [twoFaStatus]="twoFaStatus"
                  [hiddenPhoneNumber]="hiddenPhoneNumber"
                  (submitButton)="handleSubmitTwoFaForm($event)"
                  (selectOption)="handleOption($event)"
                  (resendCode)="handleResendCode()"
                ></app-two-factor>
              }
              @case ('forgot-password') {
                <app-forgot-password
                  (submitButton)="handleSubmitForgotPassword($event)"
                ></app-forgot-password>
              }
              @case ('forgot-password-confirmation') {
                <app-forgot-password-confirmation
                  (submitButton)="handleOption('login')"
                ></app-forgot-password-confirmation>
              }
              @case ('reset-password') {
                <app-reset-password
                  (submitForm)="handleSubmitResetPasswordForm($event)"
                ></app-reset-password>
              }
              @case ('reset-password-confirmation') {
                <app-reset-password-confirmation
                  (submitButton)="handleOption('login')"
                ></app-reset-password-confirmation>
              }
              @case ('reset-password-error') {
                <app-reset-password-error
                  (submitButton)="handleOption('login')"
                ></app-reset-password-error>
              }
              @case ('live-donation-collection') {
                <app-live-donation-collection></app-live-donation-collection>
              }
              @case ('change-password') {
                <app-change-password
                  (submitForm)="handleSubmitChangePasswordForm($event)"
                ></app-change-password>
              }
              @case ('change-password-confirmation') {
                <app-change-password-confirmation></app-change-password-confirmation>
              }
              @case ('change-password-error') {
                <app-change-password-error></app-change-password-error>
              }
              @case ('backup-code-login') {
                <app-backup-code-login
                  [errorMessage]="errorMessage"
                  [loading]="isLoading"
                  (submitForm)="handleUseRecoveryCode($event)"
                ></app-backup-code-login>
              }
              @case ('setup-totp') {
                <app-setup-totp
                  [loading]="isLoading"
                  [qrCodeImage]="qrCodeImage"
                  [manualKey]="manualKey"
                  [errorMessage]="errorMessage"
                  (submitForm)="handleSubmitSetupTotp($event)"
                ></app-setup-totp>
              }
              @case ('setup-sms') {
                <app-setup-sms
                  [loading]="isLoading"
                  [errorMessage]="errorMessage"
                  (submitCode)="handleSubmitSetupSms($event)"
                  (resendCode)="handleResendSetupSms()"
                ></app-setup-sms>
              }

              @case ('backup-codes') {
                <app-backup-codes
                  [codes]="backupCodes"
                  [loading]="isLoading"
                  [message]="errorMessage"
                  (regenerateCodes)="handleRegenerateBackupCodes()"
                  [errorMessage]="errorMessage"
                ></app-backup-codes>
              }
            }
          </div>
        </div>
      </div>
    }
  `,
})
export class AuthModalComponent {
  private modalService = inject(ModalService);
  private email = '';
  private phoneNumber = '';
  private firstName = '';
  private lastName = '';
  private postalCode = '';
  private password = '';

  modalState = this.modalService.modalStateReadonly;
  resetPasswordToken = signal<string | null>(null);
  resetPasswordEmail = signal<string | null>(null);
  authStep = signal<AuthStep>('login');
  qrCodeImage = signal('');
  manualKey = signal('');
  authService = inject(AuthService);

  errorMessage = signal<string>('');
  isLoading = signal(false);
  twoFaStatus = this.authService.twoFaStatus;
  hiddenPhoneNumber = signal('');
  backupCodes = signal<string[]>([]);

  constructor() {
    effect(() => {
      if (this.modalState().component === 'backup-codes') {
        this.loadBackupCodes();
      }
    });
    effect(() => {
      this.resetPasswordToken.set(this.modalService.getToken());
      this.resetPasswordEmail.set(
        this.modalService.getResettingPasswordEmail()
      );
      this.authStep.set(this.authService.getAuthStep());
      this.qrCodeImage.set(this.modalService.getQrCodeImage() || '');
      this.manualKey.set(this.modalService.getManualKey() || '');
    });
  }
  handleOption(option: ModalState['component']) {
    console.log('handleOption - ', option);
    if (option === 'backup-codes') {
      this.loadBackupCodes(); // завантажуємо коди перед відображенням
    }
    if (option === 'setup-totp') {
      this.authService.setupTotp().subscribe({
        next: response => {
          this.qrCodeImage.set(response.qrCodeImage);
          this.manualKey.set(response.manualKey);
          this.modalService.openModal('setup-totp');
          console.log('Setup TOTP success!!!!!:', response.manualKey);
        },
        error: err => {
          console.error('Setup TOTP error:', err);
          //відобразити модалку помилки
        },
      });
    }

    // Скидаємо дані при переході до welcome, login або register-email
    if (
      option &&
      ['welcome', 'login', 'register-email', 'forgot-password'].includes(option)
    ) {
      this.resetFormData();
    }
    this.modalService.openModal(option);
  }

  handleEmail($event: string) {
    this.email = $event;
  }
  handlePhoneNumber($event: string) {
    this.phoneNumber = $event;
  }

  handleFirstName($event: string) {
    this.firstName = $event;
  }

  handleLastName($event: string) {
    this.lastName = $event;
  }

  handleZipCode($event: string) {
    this.postalCode = $event;
  }

  handlePassword($event: string) {
    this.password = $event;
  }

  closeModal() {
    this.resetFormData();
    this.modalService.closeModal();
  }

  handleSubmitRegistrationForm() {
    this.authService
      .register({
        email: this.email,
        password: this.password,
        firstName: this.firstName,
        lastName: this.lastName,
        postalCode: this.postalCode,
        phone: this.phoneNumber,
      })
      .subscribe({
        next: () => {
          this.modalService.openModal('registration-confirmation');
        },
        error: err => {
          console.log(err);
          if (err.error?.error?.includes('email')) {
            this.modalService.openModal('existing-email-error');
            return;
          }
          if (err.error?.error?.includes('телефон')) {
            this.modalService.openModal('existing-email-error');
            return;
          }
          console.error('Registration error:', err);
          this.modalService.openModal('registration-failed');
        },
      });
  }
  handleSubmitLoginForm() {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.authService
      .login({
        email: this.email,
        password: this.password,
      })
      .subscribe({
        next: response => {
          if (response.user) {
            this.isLoading.set(false);
            this.modalService.closeModal();
          } else if (response.status === 'email_not_verified') {
            this.isLoading.set(false);
            this.handleResendVerificationEmail();
          } else if (response.status === '2fa_required') {
            if (
              response.hiddenPhoneNumber !== null &&
              response.hiddenPhoneNumber !== undefined &&
              response.hiddenPhoneNumber !== ''
            ) {
              this.hiddenPhoneNumber.set(response.hiddenPhoneNumber); //для прикладу
            }
            if (response.method === 'sms') {
              this.authService.sendSms2fa().subscribe({
                next: () => {
                  this.isLoading.set(false);
                  this.modalService.openModal('two-factor');
                },
                error: err => {
                  this.errorMessage.set('SEND_SMS_ERROR');
                  this.isLoading.set(false);
                  console.error('Send SMS error:', err);
                },
              });
            }
            if (response.method === 'totp') {
              this.isLoading.set(false);
              this.modalService.openModal('two-factor');
            }
          } else if (response.status === 'error') {
            this.isLoading.set(false);
            this.errorMessage.set('AUTH_ERROR');
            console.error('Login error:', response.message);
          }
          this.isLoading.set(false);
        },
        error: err => {
          this.errorMessage.set('AUTH_ERROR');
          this.isLoading.set(false);
          console.error('Login error:', err);
        },
      });
  }
  handleSubmitTwoFaForm(code: string) {
    this.isLoading.set(true);
    this.errorMessage.set('');
    const status = this.twoFaStatus();

    let request$;
    if (status?.isTwoFactorEnabled) {
      request$ = this.authService.verifyTotp(code);
    } else if (status?.isSms2FaEnabled) {
      request$ = this.authService.verifySms2fa(code);
    } else {
      this.errorMessage.set('NO_2FA_METHOD_ENABLED');
      this.isLoading.set(false);
      return;
    }

    request$.subscribe({
      next: () => {
        this.isLoading.set(false);
        this.modalService.closeModal();
      },
      error: err => {
        this.errorMessage.set('INVALID_2FA_CODE');
        this.isLoading.set(false);
        console.error('2FA error:', err);
      },
    });
  }
  handleResendCode() {
    this.isLoading.set(true);
    this.authService.sendSms2fa().subscribe({
      next: () => {
        this.isLoading.set(false);
      },
      error: err => {
        this.errorMessage.set('SEND_SMS_ERROR');
        this.isLoading.set(false);
        console.error('Send SMS error:', err);
      },
    });
  }

  handleResendVerificationEmail() {
    this.authService.resendVerification(this.email).subscribe({
      next: () => {
        this.modalService.openModal('registration-confirmation');
      },
      error: err => {
        console.error('Resend verification email error:', err);

        this.modalService.openModal('send-email-error');
      },
    });
  }

  handleSubmitForgotPassword($event: string) {
    this.isLoading.set(true);
    this.authService.forgotPassword($event).subscribe({
      next: () => {
        this.isLoading.set(false);
      },
      error: err => {
        console.error('Forgot password error:', err);
        this.isLoading.set(false);
      },
    });
    this.modalService.openModal('forgot-password-confirmation');
  }

  handleSubmitResetPasswordForm(newPassword: string) {
    if (!this.resetPasswordToken() || !this.resetPasswordEmail()) {
      this.modalService.openModal('reset-password-error');
      return;
    }

    this.authService
      .resetPassword(
        this.resetPasswordEmail()!,
        this.resetPasswordToken()!,
        newPassword
      )
      .subscribe({
        next: () => {
          this.modalService.openModal('reset-password-confirmation');
          this.isLoading.set(false);
        },
        error: err => {
          console.error('Reset password error:', err);

          this.modalService.openModal('reset-password-error');
          this.isLoading.set(false);
        },
      });
  }
  handleSubmitChangePasswordForm($newPassword: string) {
    this.authService.updateUser({ password: $newPassword }).subscribe({
      next: () => {
        this.modalService.openModal('change-password-confirmation');
        this.isLoading.set(false);
      },
      error: err => {
        console.error('Reset password error:', err);

        this.modalService.openModal('change-password-error');
        this.isLoading.set(false);
      },
    });
  }
  private resetFormData() {
    this.isLoading.set(false);
    this.errorMessage.set('');
    this.hiddenPhoneNumber.set('');
    this.email = '';
    this.firstName = '';
    this.lastName = '';
    this.postalCode = '';
    this.password = '';
  }
  handleSubmitSetupTotp(code: string) {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.authService.verifyTotpSetup(code).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.modalService.closeModal();
      },
      error: err => {
        this.errorMessage.set('INVALID_TOTP_CODE');
        this.isLoading.set(false);
        console.error('TOTP error:', err.message);
      },
    });
  }

  handleSubmitSetupSms(code: string) {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.authService.verifySmsSetup(code).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.modalService.closeModal();
      },
      error: err => {
        this.errorMessage.set('INVALID_SMS_CODE');
        this.isLoading.set(false);
        console.error('SMS 2FA error:', err.message);
      },
    });
  }

  handleResendSetupSms() {
    this.isLoading.set(true);
    this.authService.setupSms2fa().subscribe({
      next: response => {
        console.log('Setup SMS 2FA success:', response);
        this.errorMessage.set('');
        this.isLoading.set(false);
      },
      error: err => {
        console.error('Setup SMS 2FA error:', err);
        //відобразити модалку помилки
      },
    });
  }

  handleRegenerateBackupCodes() {
    this.errorMessage.set('');
    this.isLoading.set(true);
    this.authService.regenerateTotpBackupCodes().subscribe({
      next: response => {
        this.isLoading.set(false);
        this.backupCodes.set(response.backupCodes); // оновлюємо коди для компонента
      },
      error: err => {
        this.isLoading.set(false);
        this.errorMessage.set('FAILED_TO_REGENERATE_CODES');
        console.error('Backup codes regeneration error:', err);
      },
    });
  }

  loadBackupCodes() {
    this.isLoading.set(true);
    this.authService.getTotpBackupCodes().subscribe({
      next: response => {
        this.backupCodes.set(response.backupCodes);
        this.errorMessage.set('');
        this.isLoading.set(false);
      },
      error: err => {
        this.errorMessage.set('FAILED_TO_LOAD_CODES');
        this.isLoading.set(false);
        console.error('Failed to load backup codes:', err);
      },
    });
  }

  handleUseRecoveryCode($event: string) {
    this.isLoading.set(true);
    this.authService.verifyTotpBackupCode($event).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.errorMessage.set('');
        this.modalService.closeModal();
      },
      error: err => {
        console.error('Recovery code error', err.message);
        this.isLoading.set(false);
        this.errorMessage.set('BACKAP_CODE_INVALID');
      },
    });
  }
}
