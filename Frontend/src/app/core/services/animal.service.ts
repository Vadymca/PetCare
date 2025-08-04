import { Injectable, inject } from '@angular/core';
import { Observable, forkJoin, map, of, switchMap } from 'rxjs';
import { Animal } from '../models/animal';

import { ApiService } from './api.service'; // оновлена адреса

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

  // getAnimalsSimple(): Observable<Animal[]> {
  //   return this.api.get<Animal[]>(this.endpoint);
  // }

  // getAnimalSimpleBySlug(slug: string): Observable<Animal | undefined> {
  //   return this.api
  //     .getBySlug<Animal>(this.endpoint, slug)
  //     .pipe(map(animals => animals[0]));
  // }

  // getAnimalSimpleById(id: string): Observable<Animal | undefined> {
  //   return this.api.getById<Animal>(this.endpoint, id);
  // }
  getAnimalById(id: string): Observable<Animal | undefined> {
    return this.api.getById<Animal>(this.endpoint, id).pipe(
      switchMap(animal => {
        if (!animal) return of(undefined);

        const breed$ = animal.breedId
          ? this.breedService.getBreedById(animal.breedId)
          : of(undefined);
        const shelter$ = animal.shelterId
          ? this.shelterService.getShelterById(animal.shelterId)
          : of(undefined);
        const user$ = animal.userId
          ? this.userService.getUserById(animal.userId)
          : of(undefined);

        return forkJoin({
          breed: breed$,
          shelter: shelter$,
          user: user$,
        }).pipe(
          switchMap(({ breed, shelter, user }) => {
            if (!breed) return of(undefined);
            return this.speciesService.getSpeciesById(breed.speciesId).pipe(
              map(species => {
                const age = this.calculateAgeParts(animal.birthday);
                return {
                  ...animal,
                  breed,
                  species,
                  shelter,
                  user,
                  age,
                } as Animal;
              })
            );
          })
        );
      })
    );
  }

  getAnimalBySlug(slug: string): Observable<Animal | undefined> {
    return this.api
      .getBySlug<Animal>(this.endpoint, slug)
      .pipe(map(animals => animals[0]))
      .pipe(
        switchMap(animal => {
          if (!animal) return of(undefined);

          const breed$ = animal.breedId
            ? this.breedService.getBreedById(animal.breedId)
            : of(undefined);
          const shelter$ = animal.shelterId
            ? this.shelterService.getShelterById(animal.shelterId)
            : of(undefined);
          const user$ = animal.userId
            ? this.userService.getUserById(animal.userId)
            : of(undefined);

          return forkJoin({
            breed: breed$,
            shelter: shelter$,
            user: user$,
          }).pipe(
            switchMap(({ breed, shelter, user }) => {
              if (!breed) return of(undefined);
              return this.speciesService.getSpeciesById(breed.speciesId).pipe(
                map(species => {
                  const age = this.calculateAgeParts(animal.birthday);
                  return {
                    ...animal,
                    breed,
                    species,
                    shelter,
                    user,
                    age,
                  } as Animal;
                })
              );
            })
          );
        })
      );
  }

  getAnimals(): Observable<Animal[]> {
    return this.api.get<Animal[]>(this.endpoint).pipe(
      switchMap(animals => {
        if (animals.length === 0) return of([]);

        const detailedAnimals$ = animals.map(animal => {
          const breed$ = animal.breedId
            ? this.breedService.getBreedById(animal.breedId)
            : of(undefined);
          const shelter$ = animal.shelterId
            ? this.shelterService.getShelterById(animal.shelterId)
            : of(undefined);
          const user$ = animal.userId
            ? this.userService.getUserById(animal.userId)
            : of(undefined);

          return forkJoin({
            breed: breed$,
            shelter: shelter$,
            user: user$,
          }).pipe(
            switchMap(({ breed, shelter, user }) => {
              if (!breed) return of(undefined);
              return this.speciesService.getSpeciesById(breed.speciesId).pipe(
                map(species => {
                  const age = this.calculateAgeParts(animal.birthday);
                  return {
                    ...animal,
                    breed,
                    species,
                    shelter,
                    user,
                    age,
                  } as Animal;
                })
              );
            })
          );
        });

        return forkJoin(detailedAnimals$).pipe(
          map(details => details.filter((d): d is Animal => !!d))
        );
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
}
