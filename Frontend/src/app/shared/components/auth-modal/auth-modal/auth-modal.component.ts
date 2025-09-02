import { Component, effect, inject, signal } from '@angular/core';

import {
  AuthService,
  SomeResponse,
} from '../../../../core/services/auth.service';
import {
  ModalService,
  ModalState,
} from '../../../../core/services/modal.service';
import { IconComponent } from '../../icon.component';
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
  ],
  template: `
    @if (modalState().isOpen) {
      <div
        class="fixed inset-0 bg-secondary-neutral-mineShaft bg-opacity-50 flex items-center justify-center z-[99999] animate-fade-in"
      >
        <div
          class="flex flex-col bg-secondary-neutral-white text-secondary-neutral-mineShaft p-6 h-[720px] w-[576px] rounded-[40px] animate-slide-up shadow-2xl"
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
                ></app-register-email>
              }
              @case ('register-name') {
                <app-register-name
                  (selectOption)="handleOption($event)"
                  (firstName)="handleFirstName($event)"
                  (lastName)="handleLastName($event)"
                  (zipCode)="handleZipCode($event)"
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
                  [totpEnabled]="totpEnabled"
                  [smsEnabled]="smsEnabled"
                  [maskedPhoneNumber]="maskedPhoneNumber"
                  (submitButton)="handleSubmitTwoFaForm($event)"
                  (backupCode)="handleSubmitBackupCode($event)"
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
  private firstName = '';
  private lastName = '';
  private zipCode = '';
  private password = '';

  modalState = this.modalService.modalStateReadonly;
  resetPasswordToken = signal<string | null>(null);
  authService = inject(AuthService);

  errorMessage = signal<string>('');
  isLoading = signal(false);
  totpEnabled = signal(false);
  smsEnabled = signal(false);
  maskedPhoneNumber = signal('');

  constructor() {
    effect(() => {
      this.resetPasswordToken.set(this.modalService.getToken());
    });
  }
  handleOption(option: ModalState['component']) {
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

  handleFirstName($event: string) {
    this.firstName = $event;
  }

  handleLastName($event: string) {
    this.lastName = $event;
  }

  handleZipCode($event: string) {
    this.zipCode = $event;
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
        zipCode: this.zipCode,
      })
      .subscribe({
        next: () => {
          this.modalService.openModal('registration-confirmation');
        },
        error: err => {
          if (err.message === 'Email already exists') {
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

    this.authService.login(this.email, this.password).subscribe({
      next: response => {
        if (response.twoFactorRequired) {
          //велике питання чи це вже буде працювати на етапі недоавторизації, поміняти, коли буде свагер
          // this.authService.get2faStatus().subscribe({
          //   next: response => {
          //     this.totpEnabled.set(response.totpEnabled);
          //     this.smsEnabled.set(response.smsEnabled);
          //     this.maskedPhoneNumber.set(`+380*******25`); //поправити потім
          //   },
          //   error: err => {
          //     this.errorMessage.set('GET_2FA_STATUS_ERROR');
          //     console.error('Get 2FA status error:', err);
          //   },
          // });

          this.totpEnabled.set(response.totpEnabled ? true : false); //для прикладу
          this.maskedPhoneNumber.set(response.maskedPhoneNumber || ''); //для прикладу
          this.smsEnabled.set(response.smsEnabled ? true : false); //для прикладу

          this.modalService.openModal('two-factor');
        } else {
          if (response.user?.emailConfirmed) {
            this.isLoading.set(false);
            this.modalService.closeModal();
          } else {
            this.modalService.openModal('email-not-confirmed');
          }
          this.isLoading.set(false);
          this.modalService.closeModal();
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
  handleSubmitTwoFaForm($event: string) {
    this.isLoading.set(true);
    this.errorMessage.set('');
    // if (this.totpEnabled()) {
    // 	this.authService.verifyTotp(twoFaCode).subscribe({
    //     next: () => {
    //       this.isLoading.set(false);
    //       this.modalService.closeModal();
    //     },
    //     error: err => {
    //       this.errorMessage.set('INVALID_2FA_CODE');
    //       this.isLoading.set(false);
    //       console.error('2FA error:', err);
    //     },
    //   });
    // }
    // if (this.smsEnabled()) {
    // 	this.authService.verifySms2fa(twoFaCode).subscribe({
    //     next: () => {
    //       this.isLoading.set(false);
    //       this.modalService.closeModal();
    //     },
    //     error: err => {
    //       this.errorMessage.set('INVALID_2FA_CODE');
    //       this.isLoading.set(false);
    //       console.error('2FA error:', err);
    //     },
    //   });
    // }
    this.authService.verify2fa($event).subscribe({
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
  handleSubmitBackupCode($event: string) {
    this.isLoading.set(true);
    this.authService.verifyTotpBackupCode($event).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.modalService.closeModal();
      },
      error: err => {
        this.errorMessage.set('INVALID_BACKUP_CODE');
        this.isLoading.set(false);
        console.error('Backup code error:', err);
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
    if (!this.resetPasswordToken()) {
      this.modalService.openModal('reset-password-error');
      return;
    }

    this.authService
      .resetPassword(this.resetPasswordToken()!, newPassword)
      .subscribe({
        next: (response: SomeResponse) => {
          if (response.success) {
            this.modalService.openModal('reset-password-confirmation');
            this.isLoading.set(false);
          } else {
            this.modalService.openModal('reset-password-error');
            this.isLoading.set(false);
          }
        },
        error: err => {
          console.error('Reset password error:', err);

          this.modalService.openModal('reset-password-error');
          this.isLoading.set(false);
        },
      });
  }
  private resetFormData() {
    this.isLoading.set(false);
    this.errorMessage.set('');
    this.totpEnabled.set(false);
    this.smsEnabled.set(false);
    this.maskedPhoneNumber.set('');
    this.email = '';
    this.firstName = '';
    this.lastName = '';
    this.zipCode = '';
    this.password = '';
  }
}
