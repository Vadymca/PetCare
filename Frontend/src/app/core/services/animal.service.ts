import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, forkJoin, map, of, switchMap } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import { Animal } from '../interfaces/animal';
import { AnimalDetail } from '../interfaces/animal-detail';

// Імпорти інших сервісів
import { BreedService } from './breed.service';
import { ShelterService } from './shelter.service';
import { SpeciesService } from './species.service';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root',
})
export class AnimalService {
  private http = inject(HttpClient);
  private breedService = inject(BreedService);
  private shelterService = inject(ShelterService);
  private userService = inject(UserService);
  private speciesService = inject(SpeciesService);

  private baseUrl = `${API_BASE_URL}/animals`;

  getAnimals(): Observable<Animal[]> {
    return this.http.get<Animal[]>(this.baseUrl);
  }

  getAnimalBySlug(slug: string): Observable<Animal | undefined> {
    return this.http
      .get<Animal[]>(`${this.baseUrl}?slug=${slug}`)
      .pipe(map(animals => animals[0]));
  }
  getAnimalById(id: string): Observable<Animal | undefined> {
    return this.http.get<Animal>(`${this.baseUrl}/${id}`);
  }

  getAnimalDetailBySlug(slug: string): Observable<AnimalDetail | undefined> {
    return this.getAnimalBySlug(slug).pipe(
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
                } as AnimalDetail;
              })
            );
          })
        );
      })
    );
  }
  getAnimalsWithDetails(): Observable<AnimalDetail[]> {
    return this.getAnimals().pipe(
      switchMap(animals => {
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
                  } as AnimalDetail;
                })
              );
            })
          );
        });

        return forkJoin(detailedAnimals$).pipe(
          // Прибрати undefined-елементи, якщо породи немає
          map(details =>
            details.filter((detail): detail is AnimalDetail => !!detail)
          )
        );
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
