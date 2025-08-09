import { Injectable, inject } from '@angular/core';
import {
  Observable,
  catchError,
  forkJoin,
  from,
  map,
  mergeMap,
  of,
  toArray,
} from 'rxjs';

import { AnimalAidRequest } from '../models/animalAidRequest';
import { ApiService } from './api.service';
import { ShelterService } from './shelter.service';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root',
})
export class AnimalAidRequestService {
  private api = inject(ApiService);
  private endpoint = `animal-aid-requests`;
  private userService = inject(UserService);
  private shelterService = inject(ShelterService);

  getAnimalAidRequests(): Observable<AnimalAidRequest[]> {
    return this.api.get<AnimalAidRequest[]>(this.endpoint).pipe(
      mergeMap(animalAidRequests => from(animalAidRequests)),
      mergeMap(animalAidRequest => {
        const shelter$ = this.shelterService
          .getShelterById(animalAidRequest.shelterId)
          .pipe(catchError(() => of(undefined)));
        const user$ = this.userService
          .getUserById(animalAidRequest.userId)
          .pipe(catchError(() => of(undefined)));

        return forkJoin({ shelter: shelter$, user: user$ }).pipe(
          map(
            ({ shelter, user }) =>
              ({
                ...animalAidRequest,
                shelter,
                user,
              }) as AnimalAidRequest
          )
        );
      }),
      toArray()
    );
  }
  getAnimalAidRequestById(
    id: string
  ): Observable<AnimalAidRequest | undefined> {
    return this.api.getById<AnimalAidRequest>(this.endpoint, id).pipe(
      mergeMap(animalAidRequest => {
        if (!animalAidRequest) return of(undefined);
        const shelter$ = this.shelterService
          .getShelterById(animalAidRequest.shelterId)
          .pipe(catchError(() => of(undefined)));
        const user$ = this.userService
          .getUserById(animalAidRequest.userId)
          .pipe(catchError(() => of(undefined)));

        return forkJoin({ shelter: shelter$, user: user$ }).pipe(
          map(
            ({ shelter, user }) =>
              ({
                ...animalAidRequest,
                shelter,
                user,
              }) as AnimalAidRequest
          )
        );
      })
    );
  }
  createAnimalAidRequest(animalAidRequest: Partial<AnimalAidRequest>) {
    return this.api.post<AnimalAidRequest>(this.endpoint, animalAidRequest);
  }
  updateAnimalAidRequest(
    id: string,
    animalAidRequest: Partial<AnimalAidRequest>
  ) {
    return this.api.put<AnimalAidRequest>(this.endpoint, id, animalAidRequest);
  }
  deleteAnimalAidRequest(id: string): Observable<void> {
    return this.api.delete<void>(this.endpoint, id);
  }
}
