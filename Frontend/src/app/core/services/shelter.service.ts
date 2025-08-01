import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';

import { Shelter } from '../models/shelter';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root',
})
export class ShelterService {
  private api = inject(ApiService);
  private endpoint = `shelters`;

  getShelters(): Observable<Shelter[]> {
    return this.api.get<Shelter[]>(this.endpoint);
  }

  getShelterBySlug(slug: string): Observable<Shelter | undefined> {
    return this.api
      .getBySlug<Shelter>(this.endpoint, slug)
      .pipe(map(shelters => shelters[0]));
  }

  getShelterById(id: string): Observable<Shelter> {
    return this.api.getById<Shelter>(this.endpoint, id);
  }

  createShelter(shelter: Partial<Shelter>): Observable<Shelter> {
    return this.api.post<Shelter>(this.endpoint, shelter);
  }

  updateShelter(id: string, shelter: Partial<Shelter>): Observable<Shelter> {
    return this.api.put<Shelter>(this.endpoint, id, shelter);
  }

  deleteShelter(id: string): Observable<void> {
    return this.api.delete<void>(this.endpoint, id);
  }
}
