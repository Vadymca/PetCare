import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
  HttpParams,
} from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root', // standalone (не потребує модуля)
})
export class ApiService {
  private readonly BASE_URL = environment.apiUrl; // json-server

  private readonly http = inject(HttpClient); // замість конструктора

  get<T>(endpoint: string, params?: HttpParams): Observable<T> {
    return this.http
      .get<T>(`${this.BASE_URL}/${endpoint}`, { params })
      .pipe(catchError(this.handleError));
  }
  getById<T>(endpoint: string, id: string | number): Observable<T> {
    return this.http
      .get<T>(`${this.BASE_URL}/${endpoint}/${id}`)
      .pipe(catchError(this.handleError));
  }

  getBySlug<T>(endpoint: string, slug: string): Observable<T[]> {
    return this.http
      .get<T[]>(`${this.BASE_URL}/${endpoint}?slug=${slug}`)
      .pipe(catchError(this.handleError));
  }
  post<T>(
    endpoint: string,
    body: unknown,
    headers?: HttpHeaders
  ): Observable<T> {
    return this.http
      .post<T>(`${this.BASE_URL}/${endpoint}`, body, { headers })
      .pipe(catchError(this.handleError));
  }
  put<T>(endpoint: string, id: string | number, body: unknown): Observable<T> {
    return this.http
      .put<T>(`${this.BASE_URL}/${endpoint}/${id}`, body)
      .pipe(catchError(this.handleError));
  }
  patch<T>(
    endpoint: string,
    id: string | number,
    body: unknown
  ): Observable<T> {
    return this.http
      .patch<T>(`${this.BASE_URL}/${endpoint}/${id}`, body)
      .pipe(catchError(this.handleError));
  }
  delete<T>(endpoint: string, id: string | number): Observable<T> {
    return this.http
      .delete<T>(`${this.BASE_URL}/${endpoint}/${id}`)
      .pipe(catchError(this.handleError));
  }
  uploadFile<T>(endpoint: string, file: File): Observable<T> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http
      .post<T>(`${this.BASE_URL}/${endpoint}`, formData)
      .pipe(catchError(this.handleError));
  }
  uploadFiles<T>(endpoint: string, files: File[]): Observable<T> {
    const formData = new FormData();
    files.forEach(f => formData.append('files[]', f));
    return this.http
      .post<T>(`${this.BASE_URL}/${endpoint}`, formData)
      .pipe(catchError(this.handleError));
  }
  private handleError(error: HttpErrorResponse): Observable<never> {
    const isClientError = error.error instanceof ErrorEvent;

    if (isClientError) {
      console.error('Client-side error:', error.error.message);
    } else {
      console.error(
        `Backend returned code ${error.status}, body was:`,
        error.error
      );
    }

    // Можеш тут реалізувати i18n переклад повідомлень або обробку за типами помилок
    return throwError(() => new Error('Щось пішло не так. Спробуйте пізніше.'));
  }
}
