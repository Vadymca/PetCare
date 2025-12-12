import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import {
  catchError,
  map,
  Observable,
  of,
  switchMap,
  tap,
  throwError,
} from 'rxjs';
import { environment } from '../../../environments/environment';
import { User } from '../models/user';

export type AuthStep = 'login' | 'emailConfirmation' | '2fa' | 'authenticated';
export interface TwoFaStatus {
  isTwoFactorEnabled: boolean;
  isSms2FaEnabled: boolean;
}
interface LoginResponse {
  status: string;
  message?: string;
  method?: string;
  twoFaToken?: string;
  success?: boolean;

  //без двофакторки
  accessToken?: string;
  // refreshToken буде зберігатися в HttpOnly cookie (на сервері), тому його тут не треба
  user?: User;
  //двофакторка
  // twoFactorRequired?: boolean;
  // isTwoFactorEnabled?: boolean;
  // isSms2FaEnabled?: boolean;
  hiddenPhoneNumber?: string;
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
  private http = inject(HttpClient);
  private router = inject(Router);

  private readonly baseUrl = environment.apiUrl;
  //private readonly baseUrl = `${API_BASE_URL}`;

  readonly _currentUser = signal<User | null>(null);
  readonly accessToken = signal<string | null>(null);
  readonly twoFaToken = signal<string | null>(null);
  readonly authStep = signal<AuthStep>('login');
  readonly isAuthReady = signal(false);
  readonly isLoggedIn = computed(() => !!this.accessToken());
  readonly returnUrl = signal<string | null>(null);

  //qrCodeUrl = signal<string | null>(null);
  readonly backupCodes = signal<string[] | null>(null);
  readonly twoFaStatus = signal<TwoFaStatus | null>(null);

  setReturnUrl(url: string | null) {
    this.returnUrl.set(url);
  }
  getReturnUrl(): string | null {
    return this.returnUrl();
  }
  clearReturnUrl() {
    this.returnUrl.set(null);
  }

  //логін--------------------------------------------------,????? Що поверне коли треба 2ф???????доробити
  login(payload: AuthRequest): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.baseUrl}/auth/login`, payload, {
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
            if (response.twoFaToken) this.twoFaToken.set(response.twoFaToken);
          } else if (response.accessToken && response.user) {
            // Успішний логін без 2FA
            this.accessToken.set(response.accessToken);
            this._currentUser.set(response.user);
            this.twoFaToken.set(null);
            //удалити, коли бекенд поправить код
            if (response.user?.profilePhoto?.startsWith('/uploads')) {
              this._currentUser.set({
                ...response.user,
                profilePhoto: `${this.baseUrl}${response.user.profilePhoto}`,
              });
            }

            this.authStep.set('authenticated');
            this.twoFaStatus.set({
              isTwoFactorEnabled: false,
              isSms2FaEnabled: false,
            });
            const returnUrl = this.getReturnUrl(); // метод в AuthService, який зберігає URL з guard
            if (returnUrl) {
              this.router.navigateByUrl(returnUrl);
              this.clearReturnUrl(); // очищаємо після переходу
            }
          }
          this.isAuthReady.set(true);
        })
      );
  }

  //вилогінитися
  logout(): void {
    this.http
      .post(`${this.baseUrl}/auth/logout`, {}, { withCredentials: true })
      .subscribe({
        next: () => {
          this.accessToken.set(null);
          this._currentUser.set(null);
          this.twoFaStatus.set(null);
          this.twoFaToken.set(null);
          this.authStep.set('login');
          this.router.navigate(['/']);
        },
        error: () => {
          // Навіть якщо бек не відповів, все одно чистимо локальні дані
          this.accessToken.set(null);
          this._currentUser.set(null);
          this.twoFaStatus.set(null);
          this.twoFaToken.set(null);
          this.authStep.set('login');
          this.router.navigate(['/']);
        },
      });
  }

  //оновлення аксес-токена з допомогою рефреш токена++++++++++++++++++++++++++++++++++++++++++++++++++++++++
  refreshToken(): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(
        `${this.baseUrl}/auth/refresh`,
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
            if (response.user?.profilePhoto?.startsWith('/uploads')) {
              this._currentUser.set({
                ...response.user,
                profilePhoto: `${this.baseUrl}${response.user.profilePhoto}`,
              });
            }
            this.get2faStatus().subscribe({
              next: response => {
                this.twoFaStatus.set(response);
              },
              error: err => {
                console.error('Get 2FA status error:', err);
              },
            });
            this.authStep.set('authenticated');
            const returnUrl = this.getReturnUrl(); // AuthService
            if (returnUrl) {
              this.router.navigateByUrl(returnUrl);
              this.clearReturnUrl();
            }
          }
          this.isAuthReady.set(true);
        })
      );
  }

  //реєстрація нового користувача+++++++++++++++++++++++++++++++++++++++++++++++++
  register(user: Partial<User>): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}/auth/register`, user);
  }
  //збити пароль+++++++++++++++++++++++++++++++++++++++++++++++++
  forgotPassword(email: string): Observable<SomeResponse> {
    return this.http.post<SomeResponse>(
      `${this.baseUrl}/auth/forgot-password`,
      {
        email,
      }
    );
  }
  //встановлення нового паролю+++++++++++++++++++++++++++++++++++++++++++++++++?????чи додати мейл
  resetPassword(
    email: string,
    token: string,
    newPassword: string
  ): Observable<SomeResponse> {
    return this.http.post<SomeResponse>(`${this.baseUrl}/auth/reset-password`, {
      email,
      token,
      newPassword,
    });
  }
  changePassword(newPassword: string): Observable<SomeResponse> {
    return this.http.post<SomeResponse>(
      `${this.baseUrl}/auth/change-password`,
      {
        newPassword,
      }
    );
  }

  //повторна відправка токена для верифікації електронки+++++++++++++++++++++++++++++++++++++++++++++
  resendVerification(email: string): Observable<SomeResponse> {
    return this.http.post<SomeResponse>(
      `${this.baseUrl}/auth/resend-verification`,
      { email }
    );
  }
  //підтвердження електронки++++++++++++++++++++++++++++++++++++?????чи додати мейл
  verifyEmail(email: string, token: string): Observable<SomeResponse> {
    return this.http.post<SomeResponse>(`${this.baseUrl}/auth/confirm-email`, {
      email,
      token,
    });
    // .pipe(
    //   tap({
    //     next: response => {
    //       console.log('Verify email success:', response.message);
    //     },
    //     error: err => {
    //       console.log('Verify email error:', err);
    //     },
    //   })
    // )
  }

  //запуск процедури встановлення тотп в 2ф----------------------------- чи дійсно треба там recoveryCodes???
  setupTotp(): Observable<{
    qrCodeImage: string;
    manualKey: string;
  }> {
    return this.http.post<{
      qrCodeImage: string;
      manualKey: string;
    }>(`${this.baseUrl}/auth/2fa/totp/setup`, null);
  }

  //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++поки не дає кодів
  //verifyTotpSetup(code: string): POST /api/auth/2fa/totp/verify-setup
  verifyTotpSetup(code: string): Observable<{
    success: boolean;
    message: string;
    recoveryCodes: string[];
  }> {
    return this.http
      .post<{
        success: boolean;
        message: string;
        recoveryCodes: string[];
      }>(`${this.baseUrl}/auth/2fa/totp/verify-setup`, { code })
      .pipe(
        // якщо успіх, підвантажуємо актуальний статус 2FA
        switchMap(res => {
          if (res.success) {
            return this.get2faStatus().pipe(
              tap(status => {
                this.twoFaStatus.set(status);
              }),
              map(() => res) // передаємо оригінальний результат verifyTotpSetup далі
            );
          } else {
            return of(res);
          }
        })
      );
  }

  // // verifyTotp(code: string): POST /api/auth/2fa/totp/verify
  verifyTotp(code: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(
        `${this.baseUrl}/auth/2fa/totp/verify`,
        { code, twoFaToken: this.twoFaToken() },
        { withCredentials: true }
      )
      .pipe(
        tap(response => {
          if (response && response.accessToken && response.user) {
            this.accessToken.set(response.accessToken);
            this._currentUser.set(response.user);
            if (response.user?.profilePhoto?.startsWith('/uploads')) {
              this._currentUser.set({
                ...response.user,
                profilePhoto: `${this.baseUrl}${response.user.profilePhoto}`,
              });
            }

            this.get2faStatus().subscribe({
              next: response => {
                this.twoFaStatus.set(response);
              },
              error: err => {
                console.error('Get 2FA status error:', err);
              },
            });
            this.authStep.set('authenticated');
            const returnUrl = this.getReturnUrl(); // AuthService
            if (returnUrl) {
              this.router.navigateByUrl(returnUrl);
              this.clearReturnUrl();
            }
            this.twoFaToken.set(null);
          }
        })
      );
  }

  //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
  //disableTotp(): POST /api/auth/2fa/totp/disable.
  disableTotp(): Observable<{
    isTwoFactorEnabled: boolean;
    isSms2FaEnabled: boolean;
  }> {
    return this.http
      .post<{ message: string }>(`${this.baseUrl}/auth/2fa/totp/disable`, null)
      .pipe(
        switchMap(() => this.get2faStatus()),
        tap(status => this.twoFaStatus.set(status))
      );
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
    }>(`${this.baseUrl}/auth/2fa/totp/backup-codes`);
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
    }>(`${this.baseUrl}/auth/2fa/totp/regenerate-backup-codes`, null);
  }

  //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++мало би вернути все як при логіні
  //verifyTotpBackupCode(code: string): POST /api/auth/2fa/totp/verify-backup-code.
  verifyTotpBackupCode(code: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(
        `${this.baseUrl}/auth/2fa/totp/verify-backup-code`,
        { code, twoFaToken: this.twoFaToken() },
        { withCredentials: true }
      )
      .pipe(
        tap(response => {
          if (response && response.accessToken && response.user) {
            this.accessToken.set(response.accessToken);
            this._currentUser.set(response.user);
            if (response.user?.profilePhoto?.startsWith('/uploads')) {
              this._currentUser.set({
                ...response.user,
                profilePhoto: `${this.baseUrl}${response.user.profilePhoto}`,
              });
            }

            this.get2faStatus().subscribe({
              next: status => {
                this.twoFaStatus.set(status);
              },
              error: err => {
                console.error('Get 2FA status error:', err);
              },
            });

            this.authStep.set('authenticated');
            const returnUrl = this.getReturnUrl(); // AuthService
            if (returnUrl) {
              this.router.navigateByUrl(returnUrl);
              this.clearReturnUrl();
            }
            this.twoFaToken.set(null);
          }
        })
      );
  }

  //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++відправиться смс
  //setupSms2fa(phone: string): POST /api/auth/2fa/sms/setup
  setupSms2fa(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.baseUrl}/auth/2fa/sms/setup`,
      {}
    );
  }

  //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++?????????????????не зрозумілий формат відповіді
  //verifySmsSetup(code: string): POST /api/auth/2fa/sms/verify-setup.
  verifySmsSetup(code: string): Observable<{ message: string }> {
    return this.http
      .post<{
        message: string;
      }>(`${this.baseUrl}/auth/2fa/sms/verify-setup`, { code })
      .pipe(
        switchMap(res =>
          this.get2faStatus().pipe(
            tap(status => {
              this.twoFaStatus.set(status);
            }),
            map(() => res) // передаємо оригінальний результат далі
          )
        )
      );
  }

  //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
  //sendSms2fa(): POST /api/auth/2fa/sms/send
  sendSms2fa(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.baseUrl}/auth/2fa/sms/send`,
      { twoFaToken: this.twoFaToken() }
    );
  }

  //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++!!!!!!!!!!!!!!!!!!! поки не вертає юзера
  //verifySms2fa(code: string): POST /api/auth/2fa/sms/verify.
  verifySms2fa(code: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(
        `${this.baseUrl}/auth/2fa/sms/verify`,
        { code, twoFaToken: this.twoFaToken() },
        { withCredentials: true }
      )
      .pipe(
        tap(response => {
          if (response && response.accessToken && response.user) {
            this.accessToken.set(response.accessToken);
            this._currentUser.set(response.user);
            if (response.user?.profilePhoto?.startsWith('/uploads')) {
              this._currentUser.set({
                ...response.user,
                profilePhoto: `${this.baseUrl}${response.user.profilePhoto}`,
              });
            }
            this.get2faStatus().subscribe({
              next: response => {
                this.twoFaStatus.set(response);
              },
              error: err => {
                console.error('Get 2FA status error:', err);
              },
            });
            this.authStep.set('authenticated');
            const returnUrl = this.getReturnUrl(); // AuthService
            if (returnUrl) {
              this.router.navigateByUrl(returnUrl);
              this.clearReturnUrl();
            }
            this.twoFaToken.set(null);
          }
        })
      );
  }

  //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
  //disableSms2fa(): POST /api/auth/2fa/sms/disable
  disableSms2fa(): Observable<{
    isTwoFactorEnabled: boolean;
    isSms2FaEnabled: boolean;
  }> {
    return this.http
      .post<{ message: string }>(`${this.baseUrl}/auth/2fa/sms/disable`, null)
      .pipe(
        switchMap(() => this.get2faStatus()), // отримуємо актуальний статус
        tap(status => this.twoFaStatus.set(status))
      );
  }

  //отримання статусу 2ф++++++++++++++++++++++++++++++++++++++++++++++++????чи не поміняються назви
  get2faStatus(): Observable<{
    isTwoFactorEnabled: boolean;
    isSms2FaEnabled: boolean;
  }> {
    return this.http.get<{
      isTwoFactorEnabled: boolean;
      isSms2FaEnabled: boolean;
    }>(`${this.baseUrl}/auth/2fa/status`);
  }
  refresh2faStatus(): void {
    this.get2faStatus().subscribe({
      next: response => {
        this.twoFaStatus.set(response);
      },
      error: err => {
        console.error('Get 2FA status error:', err);
      },
    });
  }

  //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++але відповідь сервера не однозначна
  //disableAll2fa(): POST /api/auth/2fa/disable-all
  disableAll2fa(): Observable<{
    isTwoFactorEnabled: boolean;
    isSms2FaEnabled: boolean;
  }> {
    return this.http
      .post<{ message: string }>(`${this.baseUrl}/auth/2fa/disable-all`, null)
      .pipe(
        switchMap(() => this.get2faStatus()),
        tap(status => this.twoFaStatus.set(status))
      );
  }

  //getRecoveryCodes(): GET /api/auth/2fa/recovery-codes
  getRecoveryCodes(): Observable<{ recoveryCodes: string[] }> {
    return this.http.get<{ recoveryCodes: string[] }>(
      `${this.baseUrl}/auth/2fa/recovery-codes`
    );
  }

  //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++АЛЕ БЕКЕНД НЕ ВЕРТАЄ МЕНІ ЮЗЕРА ПОКИ, А ПИШЕ ВСЕ ОК ТИ МОЛОДЕЦЬ
  //useRecoveryCode(code: string): POST /api/auth/2fa/use-recovery-code
  useRecoveryCode(code: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(
        `${this.baseUrl}/auth/2fa/use-recovery-code`,
        { code, twoFaToken: this.twoFaToken() },
        { withCredentials: true }
      )
      .pipe(
        tap(response => {
          if (response && response.accessToken && response.user) {
            this.accessToken.set(response.accessToken);
            this._currentUser.set(response.user);
            if (response.user?.profilePhoto?.startsWith('/uploads')) {
              this._currentUser.set({
                ...response.user,
                profilePhoto: `${this.baseUrl}${response.user.profilePhoto}`,
              });
            }

            this.get2faStatus().subscribe({
              next: status => {
                this.twoFaStatus.set(status);
              },
              error: err => {
                console.error('Get 2FA status error:', err);
              },
            });

            this.authStep.set('authenticated');
            const returnUrl = this.getReturnUrl(); // AuthService
            if (returnUrl) {
              this.router.navigateByUrl(returnUrl);
              this.clearReturnUrl();
            }
            this.twoFaToken.set(null);
          }
        })
      );
  }

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
    //перевірити адресу потім
    return this.http.put<User>(`${this.baseUrl}/users/me`, { ...user }).pipe(
      tap(response => {
        this._currentUser.set(response);
        if (response?.profilePhoto?.startsWith('/uploads')) {
          this._currentUser.set({
            ...response,
            profilePhoto: `${this.baseUrl}${response.profilePhoto}`,
          });
        }
      }),
      catchError(err => {
        console.error('Update user error:', err);
        return throwError(() => err); // важливо пробросити далі
      })
    );
  }
}
