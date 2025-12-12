import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { Animal, AnimalListResult } from '../models/animal';

import { ApiService } from './api.service'; // оновлена адреса

import { AnimalFiltersDto } from '../models/animalFiltersDto';
import { BreedService } from './breed.service';
import { ShelterService } from './shelter.service';
import { SpeciesService } from './species.service';
import { UserService } from './user.service';

@Injectable({ providedIn: 'root' })
export class AnimalService {
  private api = inject(ApiService);
  private breedService = inject(BreedService);
  private shelterService = inject(ShelterService);
  private userService = inject(UserService);
  private speciesService = inject(SpeciesService);

  private readonly endpoint = 'animals';

  getAnimalBySlug(slug: string): Observable<Animal | undefined> {
    return this.api.getBySlug<Animal>(this.endpoint, slug).pipe(
      map(animal => {
        if (!animal) return undefined;

        const age = this.calculateAgeParts(animal.birthday);

        return { ...animal, age } as Animal;
      })
    );
  }

  getAnimals(filters: AnimalFiltersDto): Observable<AnimalListResult> {
    // clean перед відправкою — видаляємо undefined
    const payload = this.cleanObject(filters);

    return this.api
      .post<{
        animals: Animal[];
        totalCount: number;
      }>(`${this.endpoint}/filter`, payload)
      .pipe(
        map(response => {
          const animals = Array.isArray(response.animals)
            ? response.animals
            : [];
          return {
            animals: animals.map(animal => ({
              ...animal,
              age: this.calculateAgeParts(animal.birthday),
              isChecked: false,
            })),
            totalCount: response.totalCount ?? animals.length,
          } as AnimalListResult;
        })
      );
  }

  create(animal: Partial<Animal>): Observable<Animal> {
    return this.api.post<Animal>(this.endpoint, animal);
  }
  update(id: number, animal: Partial<Animal>): Observable<Animal> {
    return this.api.patch<Animal>(this.endpoint, id, animal);
  }
  delete(id: number): Observable<void> {
    return this.api.delete<void>(this.endpoint, id);
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
  private cleanObject<T extends object>(obj: T): T {
    const copy = { ...obj };

    (Object.keys(copy) as (keyof T)[]).forEach(key => {
      if (copy[key] === undefined) {
        delete copy[key];
      }
    });

    return copy;
  }
}
