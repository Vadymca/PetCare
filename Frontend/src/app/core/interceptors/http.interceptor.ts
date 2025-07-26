import {
  HttpErrorResponse,
  HttpInterceptorFn,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { catchError, tap, throwError } from 'rxjs';

export const httpInterceptor: HttpInterceptorFn = (
  req: HttpRequest<any>,
  next
) => {
  console.log(`[HTTP Request] ${req.method} ${req.urlWithParams}`);

  return next(req).pipe(
    tap(event => {
      if (event instanceof HttpResponse) {
        console.log(
          `[HTTP Response] ${req.method} ${req.urlWithParams}`,
          event
        );
      }
    }),
    catchError((error: HttpErrorResponse) => {
      if (error.status === 400) {
        console.error(
          `[HTTP Error 400] Bad Request at ${req.urlWithParams}`,
          error.message
        );
        // Тут можна додатково показати повідомлення користувачу
      } else if (error.status === 404) {
        console.error(
          `[HTTP Error 404] Not Found at ${req.urlWithParams}`,
          error.message
        );
        // Можна редіректити або показати кастомний компонент
      } else {
        console.error(
          `[HTTP Error ${error.status}] at ${req.urlWithParams}`,
          error.message
        );
      }
      // Прокидуємо помилку далі
      return throwError(() => error);
    })
  );
};


