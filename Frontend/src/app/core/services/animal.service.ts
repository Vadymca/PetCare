import { Injectable, inject } from '@angular/core';
import { Observable, map, tap } from 'rxjs';
import { Animal, AnimalListResult } from '../models/animal';

import { ApiService } from './api.service'; // оновлена адреса

import { HttpParams } from '@angular/common/http';
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

  getAnimalById(id: string): Observable<Animal | undefined> {
    return this.api.getById<Animal>(this.endpoint, id).pipe(
      map(animal => {
        if (!animal) return undefined;

        const age = this.calculateAgeParts(animal.birthday);

        return { ...animal, age } as Animal;
      })
    );
  }

  getAnimalBySlug(slug: string): Observable<Animal | undefined> {
    return this.api.getBySlug<Animal>(this.endpoint, slug).pipe(
      map(animal => {
        if (!animal) return undefined;

        const age = this.calculateAgeParts(animal.birthday);

        return { ...animal, age } as Animal;
      })
    );
  }

  getAnimals(filters?: {
    page?: number;
    pageSize?: number;
    genders?: string[];
    sizes?: string[];
    statuses?: string[];
    isSterilized?: boolean;
    shelterId?: string;
    specieId?: string;
  }): Observable<AnimalListResult> {
    let params = new HttpParams();

    if (filters?.page) params = params.set('page', filters.page.toString());
    if (filters?.pageSize)
      params = params.set('pageSize', filters.pageSize.toString());
    if (filters?.genders?.length)
      params = params.set('genders', filters.genders.join(','));
    if (filters?.sizes?.length)
      params = params.set('sizes', filters.sizes.join(','));
    if (filters?.statuses?.length)
      params = params.set('statuses', filters.statuses.join(','));
    if (filters?.isSterilized !== undefined)
      params = params.set('isSterilized', filters.isSterilized.toString());
    if (filters?.shelterId) params = params.set('shelterId', filters.shelterId);
    if (filters?.specieId) params = params.set('specieId', filters.specieId);

    return this.api
      .get<{ animals: Animal[]; totalCount: number }>(this.endpoint, params)
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
        }),
        tap(result =>
          console.log('Animals after processing in service:', result)
        )
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
}
