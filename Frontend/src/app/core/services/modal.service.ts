import { Injectable, signal } from '@angular/core';

export interface ModalState {
  isOpen: boolean;
  component:
    | 'welcome'
    | 'login'
    | 'register-email'
    | 'register-name'
    | 'register'
    | 'registration-confirmation'
    | 'send-email-error'
    | 'two-factor'
    | 'registration-failed'
    | 'existing-email-error'
    | 'email-confirmed'
    | 'email-not-confirmed'
    | 'forgot-password'
    | 'reset-password'
    | 'reset-password-confirmation'
    | 'forgot-password-confirmation'
    | 'reset-password-error'
    | 'live-donation-collection'
    | 'change-password'
    | 'change-password-confirmation'
    | 'change-password-error'
    | 'setup-totp'
    | 'setup-sms'
    | 'backup-codes'
    | 'backup-code-login'
    | null;
}

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  private modalState = signal<ModalState>({ isOpen: false, component: null });
  private tokenForResettingPassword = signal<string | null>(null);
  private resettingPasswordEmail = signal<string | null>(null);
  private qrCodeImage = signal<string | null>(null);
  private manualKey = signal<string | null>(null);

  getQrCodeImage() {
    return this.qrCodeImage();
  }
  setQrCodeImage(qrCodeImage: string | null) {
    this.qrCodeImage.set(qrCodeImage);
  }
  getManualKey() {
    return this.manualKey();
  }
  setManualKey(manualKey: string | null) {
    this.manualKey.set(manualKey);
  }
  setEmailForResettingPassword(email: string | null) {
    this.resettingPasswordEmail.set(email);
  }
  getResettingPasswordEmail() {
    return this.resettingPasswordEmail();
  }
  setTokenForResettingPassword(token: string | null) {
    this.tokenForResettingPassword.set(token);
  }

  getToken() {
    return this.tokenForResettingPassword();
  }

  // Публічний доступ до сигналу
  readonly modalStateReadonly = this.modalState.asReadonly();

  openModal(component: ModalState['component']) {
    this.modalState.set({
      isOpen: true,
      component,
    });
  }

  closeModal() {
    this.modalState.set({ isOpen: false, component: null });
  }
}
