import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Animal } from '../models/animal';
import { AnimalSubscription } from '../models/animalSubscription';
import { AnimalService } from './animal.service';
import { ApiService } from './api.service';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root',
})
export class AnimalSubscriptionService {
  private api = inject(ApiService);
  private endpoint = `animals`;
  private animalService = inject(AnimalService);
  private userService = inject(UserService);

  createAnimalSubscription(animalId: string): Observable<AnimalSubscription> {
    const url = `${this.endpoint}/${animalId}/subscribe`;
    return this.api.post<AnimalSubscription>(url, {});
  }

  deleteAnimalSubscription(animalId: string): Observable<void> {
    const url = `animals/${animalId}/subscribe`;
    return this.api.delete<void>(url, '');
  }
  getFavoriteAnimals(): Observable<Animal[]> {
    const url = `${this.endpoint}/favorites`;
    return this.api.get<Animal[]>(url);
  }
}
