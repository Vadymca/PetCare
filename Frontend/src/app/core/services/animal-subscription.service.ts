import { inject, Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';

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

    return this.api.get<Animal[]>(url).pipe(
      map((animals: Animal[] | null) => {
        if (!animals || !Array.isArray(animals)) {
          return [];
        }

        return animals.map(animal => ({
          ...animal,
          age: animal.birthday
            ? this.calculateAgeParts(animal.birthday)
            : undefined, // ← undefined, а не null!
          isChecked: false,
          isFavorite: true,
        }));
      }),
      catchError(err => {
        console.error('Error loading favorite animals:', err);
        return of([]);
      })
    );
  }
  private calculateAgeParts(birthday: string): [number, number] {
    const today = new Date();
    const birthdate = new Date(birthday);
    const ageInMilliseconds = today.getTime() - birthdate.getTime();
    const ageInDays = Math.floor(ageInMilliseconds / (1000 * 60 * 60 * 24));
    const years = Math.floor(ageInDays / 365);
    const months = Math.floor((ageInDays % 365) / 30);
    return [years, months];
  }
}
