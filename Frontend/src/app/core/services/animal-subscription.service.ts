import { Injectable, inject } from '@angular/core';
import {
  Observable,
  catchError,
  forkJoin,
  from,
  map,
  mergeMap,
  of,
  switchMap,
  toArray,
} from 'rxjs';

import { AnimalSubscription } from '../models/animalSubscription';
import { AnimalService } from './animal.service';
import { ApiService } from './api.service';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root',
})
export class AnimalSubscriptionService {
  private api = inject(ApiService);
  private endpoint = `animal-subscriptions`;
  private animalService = inject(AnimalService);
  private userService = inject(UserService);

  getAnimalSubscriptions(): Observable<AnimalSubscription[]> {
    return this.api.get<AnimalSubscription[]>(this.endpoint).pipe(
      mergeMap(subscriptions => from(subscriptions)),
      mergeMap(subscription => {
        const animal$ = this.animalService
          .getAnimalById(subscription.animalId)
          .pipe(catchError(() => of(undefined)));
        const user$ = this.userService
          .getUserById(subscription.userId)
          .pipe(catchError(() => of(undefined)));

        return forkJoin({ animal: animal$, user: user$ }).pipe(
          map(
            ({ animal, user }) =>
              ({
                ...subscription,
                animal,
                user,
              }) as AnimalSubscription
          )
        );
      }),
      toArray()
    );
  }

  getAnimalSubscriptionById(
    id: string
  ): Observable<AnimalSubscription | undefined> {
    return this.api.getById<AnimalSubscription>(this.endpoint, id).pipe(
      switchMap(subscription => {
        if (!subscription) return of(undefined);
        const animal$ = this.animalService.getAnimalById(subscription.animalId);
        const user$ = this.userService.getUserById(subscription.userId);

        return forkJoin({ animal: animal$, user: user$ }).pipe(
          map(
            ({ animal, user }) =>
              ({
                ...subscription,
                animal,
                user,
              }) as AnimalSubscription
          )
        );
      })
    );
  }
  getAnimalSubscriptionsByUserId(
    userId: string
  ): Observable<AnimalSubscription[]> {
    return this.getAnimalSubscriptions().pipe(
      map(subscriptions =>
        subscriptions.filter(subscription => subscription.userId === userId)
      )
    );
  }

  createAnimalSubscription(
    animalSubscription: Partial<AnimalSubscription>
  ): Observable<AnimalSubscription> {
    return this.api.post<AnimalSubscription>(this.endpoint, animalSubscription);
  }
  updateAnimalSubscription(
    id: string,
    animalSubscription: Partial<AnimalSubscription>
  ): Observable<AnimalSubscription> {
    return this.api.put<AnimalSubscription>(
      this.endpoint,
      id,
      animalSubscription
    );
  }
  deleteAnimalSubscription(id: string): Observable<void> {
    return this.api.delete<void>(this.endpoint, id);
  }
}
