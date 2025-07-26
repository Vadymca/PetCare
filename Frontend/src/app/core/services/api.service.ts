import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root' // standalone (не потребує модуля)
})
export class ApiService {
  //private readonly BASE_URL = 'http://localhost:3000'; // json-server

  //private readonly http = inject(HttpClient); // замість конструктора

  // get<T>(endpoint: string) {
  //   return this.http.get<T>(`${this.BASE_URL}/${endpoint}`);
  // }

  // getById<T>(endpoint: string, id: string | number) {
  //   return this.http.get<T>(`${this.BASE_URL}/${endpoint}/${id}`);
  // }

  // getBySlug<T>(endpoint: string, slug: string) {
  //   return this.http.get<T[]>(`${this.BASE_URL}/${endpoint}?slug=${slug}`);
  // }
}
