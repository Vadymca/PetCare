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
    // return this.http
    //   .post<T>(`${this.BASE_URL}/${endpoint}`, formData)
    //   .pipe(catchError(this.handleError));
    return this.http
      .post<T>('http://localhost:5000/api/media/upload', formData)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    if (
      typeof ErrorEvent !== 'undefined' &&
      error.error instanceof ErrorEvent
    ) {
      // Client-side error
      console.log('Client-side error:', error.error.message);
    } else {
      // Server-side or HTTP error
      console.log(`Backend returned code, body was:`, error.error);
    }
    return throwError(
      () => new Error('Something bad happened; please try again later.')
    );
  }
}
