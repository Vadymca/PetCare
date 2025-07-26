import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import { Species } from '../interfaces/species';

@Injectable({
  providedIn: 'root',
})
export class SpeciesService {
  private http = inject(HttpClient);
  private baseUrl = `${API_BASE_URL}/species`;

  getAll(): Observable<Species[]> {
    return this.http.get<Species[]>(this.baseUrl);
  }

  getSpeciesById(id: string): Observable<Species | undefined> {
    return this.http.get<Species>(`${this.baseUrl}/${id}`);
  }
}
