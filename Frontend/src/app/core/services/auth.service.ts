import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { User } from '../models/user';

export type AuthStep = 'login' | 'emailConfirmation' | '2fa' | 'authenticated';

interface LoginResponse {
  status: string;
  message?: string;
  method?: string;
  success?: boolean;

  //без двофакторки
  accessToken?: string;
  // refreshToken буде зберігатися в HttpOnly cookie (на сервері), тому його тут не треба
  user?: User;
  //двофакторка
  // twoFactorRequired?: boolean;
  // isTwoFactorEnabled?: boolean;
  // isSms2FaEnabled?: boolean;
  maskedPhoneNumber?: string;
}
interface AuthRequest {
  email: string;
  password: string;
}

export interface SomeResponse {
  message: string;
  success?: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  // [x: string]: any;
  private http = inject(HttpClient);
  private router = inject(Router);
  private readonly baseUrl = `http://localhost:5000/api/auth`;
  private readonly baseUrl2 = `http://localhost:5000/api`;
  //private readonly baseUrl = `${API_BASE_URL}/auth`;

  //мок поки бекенд не працює
  // private mockUser: User = {
  //   id: 'fb682961-29df-46ca-92df-b0bf80495a55',
  //   email: 'user1@example.com',
  //   firstName: 'Yulia',
  //   lastName: 'Kovalenko',
  //   role: 'Admin',
  //   phone: '+380671704664',
  //   points: 53,
  //   postalCode: '58029',
  //   lastLogin: '2023-01-01T00:00:00.000Z',
  //   profilePhoto:
  //     'https://i.pinimg.com/1200x/d2/d4/56/d2d4565a95f82ab36f7ba590b51c7acd.jpg',
  //   createdAt: '2023-01-01T00:00:00.000Z',
  //   updatedAt: '2023-01-01T00:00:00.000Z',
  // };

  readonly _currentUser = signal<User | null>(null);
  readonly accessToken = signal<string | null>(null);
  readonly authStep = signal<AuthStep>('login');
  readonly isAuthReady = signal(false);
  readonly isLoggedIn = computed(() => !!this.accessToken());
  //readonly emailConfirmed = computed(() => this._currentUser()?.emailConfirmed);

  //private tempSession: { email: string; password: string } | null = null;

  //2fa
  //qrCodeUrl = signal<string | null>(null);
  backupCodes = signal<string[] | null>(null);
  //errorMessage = signal<string | null>(null);
  twoFaStatus = signal<{
    isTwoFactorEnabled: boolean;
    isSms2FaEnabled: boolean;
  } | null>(null);
  //логін--------------------------------------------------,????? Що поверне коли треба 2ф???????доробити
  login(payload: AuthRequest): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.baseUrl}/login`, payload, {
        withCredentials: true,
      })
      .pipe(
        tap(response => {
          if (response.status === 'email_not_verified') {
            this.authStep.set('emailConfirmation');
          } else if (response.status === '2fa_required') {
            // Потрібна 2FA
            this.authStep.set('2fa');
            this.twoFaStatus.set({
              isTwoFactorEnabled: response.method === 'totp' ? true : false,
              isSms2FaEnabled: response.method === 'sms' ? true : false,
            });
          } else if (response.accessToken && response.user) {
            // Успішний логін без 2FA
            this.accessToken.set(response.accessToken);
            this._currentUser.set(response.user);

            this.authStep.set('authenticated');
            this.twoFaStatus.set({
              isTwoFactorEnabled: false,
              isSms2FaEnabled: false,
            });
          }
          this.isAuthReady.set(true);
        })
      );
  }

  verify2fa(code: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(
        `${this.baseUrl}/verify-2fa`,
        { code },
        { withCredentials: true }
      )
      .pipe(
        tap(res => {
          if (res.accessToken && res.user) {
            this.accessToken.set(res.accessToken);
            this._currentUser.set(res.user);
            this.get2faStatus().subscribe({
              next: response => {
                this.twoFaStatus.set(response);
                console.log('Get 2FA status success:', response);
              },
              error: err => {
                console.error('Get 2FA status error:', err);
              },
            });
            this.authStep.set('authenticated');
          }
          this.isAuthReady.set(true);
        })
      );
  }
  //вилогінитися
  logout(): void {
    this.http
      .post(`${this.baseUrl}/logout`, {}, { withCredentials: true })
      .subscribe({
        next: () => {
          this.accessToken.set(null);
          this._currentUser.set(null);
          this.twoFaStatus.set(null);
          this.authStep.set('login');
          this.router.navigate(['/']);
        },
        error: () => {
          // Навіть якщо бек не відповів, все одно чистимо локальні дані
          this.accessToken.set(null);
          this._currentUser.set(null);
          this.twoFaStatus.set(null);
          this.authStep.set('login');
          this.router.navigate(['/']);
        },
      });
  }

  //оновлення аксес-токена з допомогою рефреш токена++++++++++++++++++++++++++++++++++++++++++++++++++++++++
  refreshToken(): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(
        `${this.baseUrl}/refresh`,
        {},
        {
          withCredentials: true,
        }
      )
      .pipe(
        tap(response => {
          if (response.accessToken && response.user) {
            this.accessToken.set(response.accessToken);
            this._currentUser.set(response.user);
            this.get2faStatus().subscribe({
              next: response => {
                this.twoFaStatus.set(response);
                console.log('Get 2FA status success:', response);
              },
              error: err => {
                console.error('Get 2FA status error:', err);
              },
            });
            this.authStep.set('authenticated');
            // this.get2faStatus().subscribe({
            //   next: response => {
            //     this.twoFaStatus.set(response);
            //     console.log('Get 2FA status success:', response);
            //   },
            //   error: err => {
            //     console.error('Get 2FA status error:', err);
            //   },
            // });
          }
          this.isAuthReady.set(true);
        })
      );
  }

  //реєстрація нового користувача+++++++++++++++++++++++++++++++++++++++++++++++++
  register(user: Partial<User>): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}/register`, user);
  }
  //збити пароль+++++++++++++++++++++++++++++++++++++++++++++++++
  forgotPassword(email: string): Observable<SomeResponse> {
    return this.http.post<SomeResponse>(`${this.baseUrl}/forgot-password`, {
      email,
    });
  }
  //встановлення нового паролю+++++++++++++++++++++++++++++++++++++++++++++++++?????чи додати мейл
  resetPassword(
    email: string,
    token: string,
    newPassword: string
  ): Observable<SomeResponse> {
    return this.http.post<SomeResponse>(`${this.baseUrl}/reset-password`, {
      email,
      token,
      newPassword,
    });
  }
  changePassword(newPassword: string): Observable<SomeResponse> {
    return this.http.post<SomeResponse>(`${this.baseUrl}/change-password`, {
      newPassword,
    });
  }

  //Приклад роботи на мок-датабазі

  // login(email: string, password: string): Observable<LoginResponse> {
  //   // Заміна запиту на мок
  //   if (email === this.mockUser.email && password === 'password)<>?”!@1Q') {
  //     this.tempSession = { email, password };

  //     this.authStep.set('2fa');
  //     return of<LoginResponse>({
  //       success: true,
  //       twoFactorRequired: true,
  //       isTwoFactorEnabled: false,
  //       isSms2FaEnabled: true,
  //       maskedPhoneNumber: '+380*******25',
  //     }).pipe(delay(500));
  //   }
  //   return throwError(() => new Error('INVALID_CREDENTIALS'));
  // }

  // verify2fa(code: string): Observable<User> {
  //   if (!this.tempSession)
  //     return throwError(() => new Error('NO_LOGIN_SESSION'));

  //   if (code === '123456') {
  //     // зберігаємо токен
  //     const fakeToken = 'mock-jwt-token';
  //     this.accessToken.set(fakeToken);

  //     this._currentUser.set(this.mockUser);
  //     this.authStep.set('authenticated');

  //     // у реальному випадку тут би був refresh токен у cookie
  //     return of(this.mockUser).pipe(delay(500));
  //   }

  //   return throwError(() => new Error('INVALID_2FA_CODE'));
  // }
  // logout(): void {
  //   this.accessToken.set(null);
  //   this._currentUser.set(null);
  //   this.authStep.set('login');
  //   this.tempSession = null;
  //   this.router.navigate(['/']);
  // }
  // refreshToken(): Observable<{ accessToken: string }> {
  //   // Імітація затримки відповіді, наприклад 500ms
  //   const fakeAccessToken = 'mocked-access-token-12345';

  //   return of({ accessToken: fakeAccessToken }).pipe(
  //     delay(500), // імітуємо мережеву затримку
  //     tap(response => {
  //       console.log('AUTH SERVICE:Refresh token:', response.accessToken);
  //       //this.accessToken.set(response.accessToken);
  //       //this.currentUser.set(this.mockUser);
  //     })
  //   );
  // }

  //повторна відправка токена для верифікації електронки+++++++++++++++++++++++++++++++++++++++++++++
  resendVerification(email: string): Observable<SomeResponse> {
    return this.http
      .post<SomeResponse>(`${this.baseUrl}/resend-verification`, { email })
      .pipe(
        tap({
          next: response => {
            console.log('Resend verification email success:', response.message);
          },
          error: err => {
            console.log('Resend verification email error:', err);
          },
        })
      );
  }
  //підтвердження електронки++++++++++++++++++++++++++++++++++++?????чи додати мейл
  verifyEmail(email: string, token: string): Observable<SomeResponse> {
    //const payload = { email, token };
    // console.log('Payload before sending:', payload);
    return this.http
      .post<SomeResponse>(`${this.baseUrl}/confirm-email`, { email, token })
      .pipe(
        tap({
          next: response => {
            console.log('Verify email success:', response.message);
          },
          error: err => {
            console.log('Verify email error:', err);
          },
        })
      );
  }

  //запуск процедури встановлення тотп в 2ф----------------------------- чи дійсно треба там recoveryCodes???
  setupTotp(): Observable<{
    qrCodeImage: string;
    manualKey: string;
    recoveryCodes: string[];
  }> {
    return this.http.post<{
      qrCodeImage: string;
      manualKey: string;
      recoveryCodes: string[];
    }>(`${this.baseUrl}/2fa/totp/setup`, null);
  }
  // Виклик у компоненті
  // initiateTotpSetup() {
  //   this.setupTotp().subscribe({
  //     next: response => {
  //       this.qrCodeUrl.set(response.qrCodeImage);
  //       console.log('Setup TOTP success:', response);
  //     },
  //     error: err => {
  //       console.error('Setup TOTP error:', err);
  //     },
  //   });
  // }
  //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++поки не дає кодів
  //verifyTotpSetup(code: string): POST /api/auth/2fa/totp/verify-setup
  verifyTotpSetup(code: string): Observable<{
    success: boolean;
    message: string;
    recoveryCodes: string[];
  }> {
    return this.http.post<{
      success: boolean;
      message: string;
      recoveryCodes: string[];
    }>(`${this.baseUrl}/2fa/totp/verify-setup`, { code });
  }
  // Виклик у компоненті
  // confirmTotpSetup(code: string) {
  //   this.verifyTotpSetup(code).subscribe({
  //     next: response => {
  //       this.backupCodes.set(response.backupCodes);
  //       console.log('Verify TOTP setup success:', response);
  //     },
  //     error: err => {
  //       console.error('Verify TOTP setup error:', err);
  //     },
  //   });
  // }
  // // verifyTotp(code: string): POST /api/auth/2fa/totp/verify
  verifyTotp(code: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(
        `${this.baseUrl}/2fa/totp/verify`,
        { code },
        { withCredentials: true }
      )
      .pipe(
        tap(response => {
          if (response && response.accessToken && response.user) {
            this.accessToken.set(response.accessToken);
            this._currentUser.set(response.user);
            this.get2faStatus().subscribe({
              next: response => {
                this.twoFaStatus.set(response);
                console.log('Get 2FA status success:', response);
              },
              error: err => {
                console.error('Get 2FA status error:', err);
              },
            });
            this.authStep.set('authenticated');
            this.authStep.set('authenticated');
          }
        })
      );
  }

  // Виклик у компоненті
  // confirmTotpLogin(code: string) {
  //   this.verifyTotp(code).subscribe({
  //     next: response => {
  //       console.log('Verify TOTP success:', response);
  //       if (response && response.accessToken) {
  //         //this.router.navigate(['/dashboard']);
  //         //this.modalService.closeModal();
  //       }
  //     },
  //     error: err => {
  //       console.error('Verify TOTP error:', err);
  //     },
  //   });
  // }
  //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
  //disableTotp(): POST /api/auth/2fa/totp/disable.
  disableTotp(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.baseUrl}/2fa/totp/disable`,
      null
    );
  }
  // Виклик у компоненті
  disableTotpAuth() {
    this.disableTotp().subscribe({
      next: response => {
        this.twoFaStatus.update(status =>
          status ? { ...status, isTwoFactorEnabled: false } : null
        );
        console.log('Disable TOTP success:', response);
      },
      error: err => {
        console.error('Disable TOTP error:', err);
      },
    });
  }
  //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
  //getTotpBackupCodes(): GET /api/auth/2fa/totp/backup-codes
  getTotpBackupCodes(): Observable<{
    success: boolean;
    message: string;
    backupCodes: string[];
  }> {
    return this.http.get<{
      success: boolean;
      message: string;
      backupCodes: string[];
    }>(`${this.baseUrl}/2fa/totp/backup-codes`);
  }
  // Виклик у компоненті
  fetchTotpBackupCodes() {
    this.getTotpBackupCodes().subscribe({
      next: response => {
        this.backupCodes.set(response.backupCodes);
        console.log('Get TOTP backup codes success:', response);
      },
      error: err => {
        console.error('Get TOTP backup codes error:', err);
      },
    });
  }
  //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
  //regenerateTotpBackupCodes(): POST /api/auth/2fa/totp/regenerate-backup-codes
  regenerateTotpBackupCodes(): Observable<{
    success: boolean;
    message: string;
    backupCodes: string[];
  }> {
    return this.http.post<{
      success: boolean;
      message: string;
      backupCodes: string[];
    }>(`${this.baseUrl}/2fa/totp/regenerate-backup-codes`, null);
  }
  // Виклик у компоненті
  // regenerateBackupCodes() {
  //   this.regenerateTotpBackupCodes().subscribe({
  //     next: response => {
  //       this.backupCodes.set(response.backupCodes);
  //       console.log('Regenerate TOTP backup codes success:', response);
  //     },
  //     error: err => {
  //       console.error('Regenerate TOTP backup codes error:', err);
  //     },
  //   });
  // }
  //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++мало би вернути все як при логіні
  //verifyTotpBackupCode(code: string): POST /api/auth/2fa/totp/verify-backup-code.
  verifyTotpBackupCode(code: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${this.baseUrl}/2fa/totp/verify-backup-code`,
      { code }
    );
  }

  // Виклик у компоненті
  confirmTotpBackupCode(code: string) {
    this.verifyTotpBackupCode(code).subscribe({
      next: response => {
        console.log('Verify TOTP backup code success:', response);
      },
      error: err => {
        console.error('Verify TOTP backup code error:', err);
      },
    });
  }
  //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++відправиться смс
  //setupSms2fa(phone: string): POST /api/auth/2fa/sms/setup
  setupSms2fa(phone: string): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.baseUrl}/2fa/sms/setup`,
      { phone }
    );
  }

  // Виклик у компоненті
  initiateSms2fa(phone: string) {
    this.setupSms2fa(phone).subscribe({
      next: response => {
        console.log('Setup SMS 2FA success:', response);
      },
      error: err => {
        console.error('Setup SMS 2FA error:', err);
      },
    });
  }
  //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++?????????????????не зрозумілий формат відповіді
  //verifySmsSetup(code: string): POST /api/auth/2fa/sms/verify-setup.
  verifySmsSetup(code: string): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.baseUrl}/2fa/sms/verify-setup`,
      { code }
    );
  }

  // Виклик у компоненті
  confirmSmsSetup(code: string) {
    this.verifySmsSetup(code).subscribe({
      next: response => {
        this.twoFaStatus.update(status =>
          status ? { ...status, isSms2FaEnabled: true } : null
        );
        console.log('Verify SMS setup success:', response);
      },
      error: err => {
        console.error('Verify SMS setup error:', err);
      },
    });
  }
  //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
  //sendSms2fa(): POST /api/auth/2fa/sms/send
  sendSms2fa(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.baseUrl}/2fa/sms/send`,
      null
    );
  }

  // Виклик у компоненті
  // initiateSms2faLogin() {
  //   this.sendSms2fa().subscribe({
  //     next: response => {
  //       console.log('Send SMS 2FA success:', response);
  //     },
  //     error: err => {
  //       console.error('Send SMS 2FA error:', err);
  //     },
  //   });
  // }
  //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++!!!!!!!!!!!!!!!!!!! поки не вертає юзера
  //verifySms2fa(code: string): POST /api/auth/2fa/sms/verify.
  verifySms2fa(code: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(
        `${this.baseUrl}/2fa/sms/verify`,
        { code },
        { withCredentials: true }
      )
      .pipe(
        tap(response => {
          if (response && response.accessToken && response.user) {
            this.accessToken.set(response.accessToken);
            this._currentUser.set(response.user);
            this.get2faStatus().subscribe({
              next: response => {
                this.twoFaStatus.set(response);
                console.log('Get 2FA status success:', response);
              },
              error: err => {
                console.error('Get 2FA status error:', err);
              },
            });
            this.authStep.set('authenticated');
            this.authStep.set('authenticated');
          }
        })
      );
  }
  // confirmSms2fa(code: string) {
  //   this.verifySms2fa(code).subscribe({
  //     next: response => {
  //       console.log('Verify SMS 2FA success:', response);
  //       if (response && response.accessToken) {
  //         this.router.navigate(['/dashboard']);
  //         this.modalService.closeModal();
  //       }
  //     },
  //     error: err => {

  //       console.error('Verify SMS 2FA error:', err);
  //     },
  //   });
  // }
  //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
  //disableSms2fa(): POST /api/auth/2fa/sms/disable
  disableSms2fa(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.baseUrl}/2fa/sms/disable`,
      null
    );
  }

  // Виклик у компоненті
  // disableSms2faAuth() {
  //   this.disableSms2fa().subscribe({
  //     next: response => {
  //       this.twoFaStatus.update(status =>
  //         status ? { ...status, isSms2FaEnabled: false } : null
  //       );
  //       console.log('Disable SMS 2FA success:', response);
  //     },
  //     error: err => {
  //       console.error('Disable SMS 2FA error:', err);
  //     },
  //   });
  // }
  //отримання статусу 2ф++++++++++++++++++++++++++++++++++++++++++++++++????чи не поміняються назви
  get2faStatus(): Observable<{
    isTwoFactorEnabled: boolean;
    isSms2FaEnabled: boolean;
  }> {
    return this.http.get<{
      isTwoFactorEnabled: boolean;
      isSms2FaEnabled: boolean;
    }>(`${this.baseUrl}/2fa/status`);
  }
  refresh2faStatus(): void {
    this.get2faStatus().subscribe({
      next: response => {
        this.twoFaStatus.set(response);
        console.log('Get 2FA status success:', response);
      },
      error: err => {
        console.error('Get 2FA status error:', err);
      },
    });
  }
  // Виклик у компоненті
  // fetch2faStatus() {
  //   this.get2faStatus().subscribe({
  //     next: response => {
  //       this.twoFaStatus.set(response);
  //       console.log('Get 2FA status success:', response);
  //     },
  //     error: err => {

  //       console.error('Get 2FA status error:', err);
  //     },
  //   });
  // }
  //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++але відповідь сервера не однозначна
  //disableAll2fa(): POST /api/auth/2fa/disable-all
  disableAll2fa(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.baseUrl}/2fa/disable-all`,
      null
    );
  }

  // Виклик у компоненті
  // disableAll2faAuth() {
  //   this.disableAll2fa().subscribe({
  //     next: response => {
  //       this.twoFaStatus.set({
  //         isTwoFactorEnabled: false,
  //         isSms2FaEnabled: false,
  //       });
  //       console.log('Disable all 2FA success:', response);
  //     },
  //     error: err => {
  //       console.error('Disable all 2FA error:', err);
  //     },
  //   });
  // }
  //getRecoveryCodes(): GET /api/auth/2fa/recovery-codes
  getRecoveryCodes(): Observable<{ recoveryCodes: string[] }> {
    return this.http.get<{ recoveryCodes: string[] }>(
      `${this.baseUrl}/2fa/recovery-codes`
    );
  }

  // Виклик у компоненті
  fetchRecoveryCodes() {
    this.getRecoveryCodes().subscribe({
      next: response => {
        this.backupCodes.set(response.recoveryCodes);
        console.log('Get recovery codes success:', response);
      },
      error: err => {
        console.error('Get recovery codes error:', err);
      },
    });
  }
  //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++АЛЕ БЕКЕНД НЕ ВЕРТАЄ МЕНІ ЮЗЕРА ПОКИ, А ПИШЕ ВСЕ ОК ТИ МОЛОДЕЦЬ
  //useRecoveryCode(code: string): POST /api/auth/2fa/use-recovery-code
  useRecoveryCode(code: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${this.baseUrl}/2fa/use-recovery-code`,
      { code }
    );
  }

  // Виклик у компоненті
  // confirmRecoveryCode(code: string) {
  //   this.useRecoveryCode(code).subscribe({
  //     next: response => {
  //       console.log('Use recovery code success:', response);
  //     },
  //     error: err => {
  //       console.error('Use recovery code error:', err);
  //     },
  //   });
  // }
  getAccessToken(): string | null {
    return this.accessToken();
  }
  getCurrentUser(): User | null {
    return this._currentUser();
  }

  getIsLoggedIn(): boolean {
    return this.isLoggedIn();
  }
  setAccessToken(token: string | null): void {
    this.accessToken.set(token);
  }
  getAuthStep(): AuthStep {
    return this.authStep();
  }
  //перевірити
  updateUser(user: Partial<User>): Observable<User> {
    if (!this._currentUser()) {
      throw new Error('No current user');
    }

    return this.http.put<User>(`${this.baseUrl2}/users/me`, { ...user }).pipe(
      tap(response => {
        this._currentUser.set(response);
        console.log('Update user success:', response);
      }),
      catchError(err => {
        console.error('Update user error:', err);
        return throwError(() => err); // важливо пробросити далі
      })
    );
  }
}
