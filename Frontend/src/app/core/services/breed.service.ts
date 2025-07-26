import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import { Breed } from '../interfaces/breed';

@Injectable({
  providedIn: 'root',
})
export class BreedService {
  private http = inject(HttpClient);
  private baseUrl = `${API_BASE_URL}/breeds`;

  getAll(): Observable<Breed[]> {
    return this.http.get<Breed[]>(this.baseUrl);
  }

  getBreedById(id: string): Observable<Breed | undefined> {
    return this.http.get<Breed>(`${this.baseUrl}/${id}`);
  }
}