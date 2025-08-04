import { Injectable, inject } from '@angular/core';
import { Observable, forkJoin, map, of, switchMap } from 'rxjs';

import { LostPet } from '../models/lostPet';
import { ApiService } from './api.service';
import { BreedService } from './breed.service';
import { SpeciesService } from './species.service';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root',
})
export class LostPetService {
  private api = inject(ApiService);
  private endpoint = `lost-pets`;
  private userService = inject(UserService);
  private breedService = inject(BreedService);
  private speciesService = inject(SpeciesService);

  getLostPets(): Observable<LostPet[]> {
    return this.api.get<LostPet[]>(this.endpoint);
  }
  getLostPetById(id: string): Observable<LostPet | undefined> {
    return this.api.getById<LostPet>(this.endpoint, id).pipe(
      switchMap(lostPet => {
        if (!lostPet) return of(undefined);
        const breed$ = lostPet.breedId
          ? this.breedService.getBreedById(lostPet.breedId)
          : of(undefined);

        const user$ = lostPet.userId
          ? this.userService.getUserById(lostPet.userId)
          : of(undefined);

        return forkJoin({ breed: breed$, user: user$ }).pipe(
          switchMap(({ breed, user }) => {
            if (!breed) return of(undefined);
            return this.speciesService.getSpeciesById(breed.speciesId).pipe(
              map(species => {
                return {
                  ...lostPet,
                  breed,
                  species,

                  user,
                } as LostPet;
              })
            );
          })
        );
      })
    );
  }
  getLostPetBySlug(slug: string): Observable<LostPet | undefined> {
    return this.api
      .getBySlug<LostPet>(this.endpoint, slug)
      .pipe(map(lostPet => lostPet[0]))
      .pipe(
        switchMap(lostPet => {
          if (!lostPet) return of(undefined);

          const breed$ = lostPet.breedId
            ? this.breedService.getBreedById(lostPet.breedId)
            : of(undefined);

          const user$ = lostPet.userId
            ? this.userService.getUserById(lostPet.userId)
            : of(undefined);

          return forkJoin({ breed: breed$, user: user$ }).pipe(
            switchMap(({ breed, user }) => {
              if (!breed) return of(undefined);

              return this.speciesService.getSpeciesById(breed.speciesId).pipe(
                map(
                  species =>
                    ({
                      ...lostPet,
                      breed,
                      species,
                      user,
                    }) as LostPet
                )
              );
            })
          );
        })
      );
  }

  getLostPetsByUserId(userId: string): Observable<LostPet[]> {
    return this.getLostPets().pipe(
      map(lostPets => lostPets.filter(lostPet => lostPet.userId === userId))
    );
  }
  createLostPet(lostPet: Partial<LostPet>): Observable<LostPet> {
    return this.api.post<LostPet>(this.endpoint, lostPet);
  }
  updateLostPet(id: string, lostPet: Partial<LostPet>): Observable<LostPet> {
    return this.api.put<LostPet>(this.endpoint, id, lostPet);
  }
  deleteLostPet(id: string): Observable<void> {
    return this.api.delete<void>(this.endpoint, id);
  }
}
