import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { delay, Observable, of, tap, throwError } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import { User } from '../models/user';

export type AuthStep = 'login' | '2fa' | 'authenticated';
interface AuthResponse {
  accessToken: string;
  // refreshToken буде зберігатися в HttpOnly cookie (на сервері), тому його тут не треба
  user: User;
}
interface AuthRequest {
  email: string;
  password: string;
}
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private readonly baseUrl = `${API_BASE_URL}/auth`;

  //мок поки бекенд не працює
  private mockUser: User = {
    id: 'fb682961-29df-46ca-92df-b0bf80495a55',
    email: 'user1@example.com',
    firstName: 'Yulia',
    lastName: 'Kovalenko',
    role: 'Admin',
    phone: '+380671704664',
    points: 53,
    lastLogin: '2023-01-01T00:00:00.000Z',
    profilePhoto:
      'https://i.pinimg.com/1200x/d2/d4/56/d2d4565a95f82ab36f7ba590b51c7acd.jpg',
    createdAt: '2023-01-01T00:00:00.000Z',
    updatedAt: '2023-01-01T00:00:00.000Z',
  };

  readonly currentUser = signal<User | null>(null);
  readonly accessToken = signal<string | null>(null);
  readonly authStep = signal<AuthStep>('login');
  readonly isLoggedIn = computed(() => !!this.accessToken());

  private tempSession: { email: string; password: string } | null = null;

  //справжній бекенд буде використовувати ці методи
  // login(payload: AuthRequest): Observable<'2fa_required'> {
  //   return this.http
  //     .post(`${this.baseUrl}/login`, payload, { withCredentials: true })
  //     .pipe(
  //       map(() => '2fa_required') // якщо сервер так відповідає
  //     );
  // }
  // verify2fa(code: string): Observable<AuthResponse> {
  //   return this.http
  //     .post<AuthResponse>(
  //       `${this.baseUrl}/verify-2fa`,
  //       { code },
  //       { withCredentials: true }
  //     )
  //     .pipe(
  //       tap(res => {
  //         this.accessToken.set(res.accessToken);
  //         this.currentUser.set(res.user);
  //         this.authStep.set('authenticated');
  //       })
  //     );
  // }
  // logout(): void {
  //   this.http
  //     .post(`${this.baseUrl}/logout`, {}, { withCredentials: true })
  //     .subscribe({
  //       next: () => {
  //         this.accessToken.set(null);
  //         this.currentUser.set(null);
  //         this.authStep.set('login');
  //         this.router.navigate(['/login']);
  //       },
  //       error: () => {
  //         // Навіть якщо бек не відповів, все одно чистимо локальні дані
  //         this.accessToken.set(null);
  //         this.currentUser.set(null);
  //         this.authStep.set('login');
  //         this.router.navigate(['/login']);
  //       },
  //     });
  // }

  // refreshToken():  Observable<AuthResponse> {
  //   return this.http
  //     .post<AuthResponse>(
  //       `${this.baseUrl}/refresh`,
  //       {},
  //       {
  //         withCredentials: true,
  //       }
  //     )
  //     .pipe(
  //       tap(response => {
  //         this.accessToken.set(response.accessToken);
  // 				this.currentUser.set(response.user);
  //       })
  //     );
  // }

  register(user: Partial<User>): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}/register`, user);
  }
  forgotPassword(email: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/forgot-password`, { email });
  }
  resetPassword(token: string, newPassword: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/reset-password`, {
      token,
      newPassword,
    });
  }
  changePassword(newPassword: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/change-password`, {
      newPassword,
    });
  }

  //Приклад роботи на мок-датабазі

  login(email: string, password: string): Observable<'2fa_required'> {
    console.log(this.mockUser.email);
    // Заміна запиту на мок
    if (email === this.mockUser.email && password === 'password') {
      console.log('AUTH SERVICE:Login attempt:', email, password);
      this.tempSession = { email, password };

      this.authStep.set('2fa');

      return of<'2fa_required'>('2fa_required').pipe(delay(500));
    }
    return throwError(() => new Error('INVALID_CREDENTIALS'));
  }

  verify2fa(code: string): Observable<User> {
    if (!this.tempSession)
      return throwError(() => new Error('NO_LOGIN_SESSION'));

    if (code === '123456') {
      // зберігаємо токен
      const fakeToken = 'mock-jwt-token';
      this.accessToken.set(fakeToken);

      this.currentUser.set(this.mockUser);
      this.authStep.set('authenticated');
      console.log('AUTH SERVICE:User:', this.currentUser.name);

      // у реальному випадку тут би був refresh токен у cookie
      return of(this.mockUser).pipe(delay(500));
    }

    return throwError(() => new Error('INVALID_2FA_CODE'));
  }
  logout(): void {
    this.accessToken.set(null);
    this.currentUser.set(null);
    this.authStep.set('login');
    this.tempSession = null;
    this.router.navigate(['/login']);
  }
  refreshToken(): Observable<{ accessToken: string }> {
    // Імітація затримки відповіді, наприклад 500ms
    const fakeAccessToken = 'mocked-access-token-12345';

    return of({ accessToken: fakeAccessToken }).pipe(
      delay(500), // імітуємо мережеву затримку
      tap(response => {
        //this.accessToken.set(response.accessToken);
        //this.currentUser.set(this.mockUser);
      })
    );
  }

  //Спільні методи для справжнього сервера і мокдатабаз

  getAccessToken(): string | null {
    return this.accessToken();
  }
  getCurrentUser(): User | null {
    return this.currentUser();
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
}
