import { Injectable, inject } from '@angular/core';
import { Observable, forkJoin, map, of, switchMap } from 'rxjs';

import { ShelterSubscription } from '../models/shelterSubscriptions';
import { ApiService } from './api.service';
import { ShelterService } from './shelter.service';
import { UserService } from './user.service';
import { Shelter } from '../models/shelter';

@Injectable({
  providedIn: 'root',
})
export class ShelterSubscriptionService {
  private api = inject(ApiService);
  private endpoint = `shelter-subscriptions`;
  private shelterService = inject(ShelterService);
  private userService = inject(UserService);

  getShelterSubscriptions(): Observable<ShelterSubscription[]> {
    return this.api.get<ShelterSubscription[]>(this.endpoint).pipe(
      switchMap(subscriptions => {
        const enriched$ = subscriptions.map(subscription => {
          const shelter$ = subscription.shelterId
            ? this.shelterService.getShelterById(subscription.shelterId)
            : of(undefined as Shelter | undefined);
          const user$ = this.userService.getUserById(subscription.userId);

          return forkJoin({ shelter: shelter$, user: user$ }).pipe(
            map(
              ({ shelter, user }) =>
                ({
                  ...subscription,
                  shelter,
                  user,
                }) as ShelterSubscription	
            )
          );
        });

        return forkJoin(enriched$); // повертаємо масив підписок з тваринкою та користувачем
      })
    );
  }
  getShelterSubscriptionsById(
    id: string
  ): Observable<ShelterSubscription | undefined> {
    return this.api.getById<ShelterSubscription>(this.endpoint, id).pipe(
      switchMap(subscription => {
        if (!subscription) return of(undefined);
        const shelter$ = this.shelterService.getShelterById(
          subscription.shelterId
        );
        const user$ = this.userService.getUserById(subscription.userId);

        return forkJoin({ shelter: shelter$, user: user$ }).pipe(
          map(
            ({ shelter, user }) =>
              ({
                ...subscription,
                shelter,
                user,
              }) as ShelterSubscription
          )
        );
      })
    );
  }
  getShelterSubscriptionsByUserId(
    userId: string
  ): Observable<ShelterSubscription[]> {
    return this.getShelterSubscriptions().pipe(
      map(subscriptions =>
        subscriptions.filter(subscription => subscription.userId === userId)
      )
    );
  }
  createShelterSubscription(
    shelterSubscription: Partial<ShelterSubscription>
  ): Observable<ShelterSubscription> {
    return this.api.post<ShelterSubscription>(
      this.endpoint,
      shelterSubscription
    );
  }
  updateShelterSubscription(
    id: string,
    shelterSubscription: Partial<ShelterSubscription>
  ): Observable<ShelterSubscription> {
    return this.api.put<ShelterSubscription>(
      this.endpoint,
      id,
      shelterSubscription
    );
  }
  deleteShelterSubscription(id: string): Observable<void> {
    return this.api.delete<void>(this.endpoint, id);
  }
}
