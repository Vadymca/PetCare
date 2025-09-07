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
    | null;
}

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  private modalState = signal<ModalState>({ isOpen: false, component: null });
  private token = signal<string | null>(null);
  setToken(token: string | null) {
    this.token.set(token);
  }
  getToken() {
    return this.token();
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
