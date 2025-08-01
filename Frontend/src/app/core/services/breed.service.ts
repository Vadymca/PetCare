import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { Breed } from '../models/breed';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root',
})
export class BreedService {
  private api = inject(ApiService);
  private endpoint = `breeds`;

  getAll(): Observable<Breed[]> {
    return this.api.get<Breed[]>(this.endpoint);
  }

  getBreedById(id: string): Observable<Breed> {
    return this.api.getById<Breed>(this.endpoint, id);
  }

  createBreed(breed: Partial<Breed>): Observable<Breed> {
    return this.api.post<Breed>(this.endpoint, breed);
  }

  updateBreed(id: string, breed: Partial<Breed>): Observable<Breed> {
    return this.api.put<Breed>(this.endpoint, id, breed);
  }

  deleteBreed(id: string): Observable<void> {
    return this.api.delete<void>(this.endpoint, id);
  }
}
