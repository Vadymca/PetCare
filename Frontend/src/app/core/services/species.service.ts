import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { Species } from '../interfaces/species';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root',
})
export class SpeciesService {
  private api = inject(ApiService);

  private endpoint = `species`;

  getAll(): Observable<Species[]> {
    return this.api.get<Species[]>(this.endpoint);
  }

  getSpeciesById(id: string): Observable<Species> {
    return this.api.getById<Species>(this.endpoint, id);
  }

  createSpecies(species: Partial<Species>): Observable<Species> {
    return this.api.post<Species>(this.endpoint, species);
  }

  updateSpecies(id: string, species: Partial<Species>): Observable<Species> {
    return this.api.put<Species>(this.endpoint, id, species);
  }

  deleteSpecies(id: string): Observable<void> {
    return this.api.delete<void>(this.endpoint, id);
  }
}
