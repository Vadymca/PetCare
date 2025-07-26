import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import { Shelter } from '../interfaces/shelter';

@Injectable({
  providedIn: 'root',
})
export class ShelterService {
  private http = inject(HttpClient);
  private baseUrl = `${API_BASE_URL}/shelters`;

  getShelters(): Observable<Shelter[]> {
    return this.http.get<Shelter[]>(this.baseUrl);
  }

  getShelterBySlug(slug: string): Observable<Shelter | undefined> {
    return this.http
      .get<Shelter[]>(`${this.baseUrl}?slug=${slug}`)
      .pipe(map(shelters => shelters[0]));
  }
  getShelterById(id: string): Observable<Shelter | undefined> {
    return this.http.get<Shelter>(`${this.baseUrl}/${id}`);
  }
}
