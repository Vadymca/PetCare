import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, from, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const httpInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  return from(Promise.resolve(authService.getAccessToken())).pipe(
    switchMap(token => {
      // Запит з access token в Authorization, але без withCredentials
      const authReq = token
        ? req.clone({
            setHeaders: { Authorization: `Bearer ${token}` },
            // without withCredentials, щоб не відправляти куки без потреби
          })
        : req;

      return next(authReq).pipe(
        catchError(error => {
          if (error.status === 401) {
            // Тепер робимо запит оновлення токена з withCredentials: true (щоб браузер додав refresh token)
            return authService.refreshToken().pipe(
              switchMap(() => {
                // Токен вже встановлений у refreshToken через tap
                const newToken = authService.getAccessToken();
                const retryReq = req.clone({
                  setHeaders: { Authorization: `Bearer ${newToken}` },
                });

                return next(retryReq);
              }),
              catchError(refreshError => {
                authService.logout();
                return throwError(() => refreshError);
              })
            );
          }
          if (error.status === 500) {
            router.navigate(['/internal-server-error']);
          }

          if (error.status === 503) {
            router.navigate(['/service-unavailable']);
          }
          return throwError(() => error);
        })
      );
    })
  );
};
