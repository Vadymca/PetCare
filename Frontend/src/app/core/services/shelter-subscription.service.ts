import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { Shelter } from '../models/shelter';
import { ShelterSubscription } from '../models/shelterSubscriptions';
import { ApiService } from './api.service';
import { ShelterService } from './shelter.service';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root',
})
export class ShelterSubscriptionService {
  private api = inject(ApiService);
  private endpoint = `shelters`;
  private shelterService = inject(ShelterService);
  private userService = inject(UserService);

  getMyFavouriteShelters(): Observable<Shelter[]> {
    return this.api.get<Shelter[]>(`${this.endpoint}/favorites`);
  }

  createShelterSubscription(
    shelterId: string
  ): Observable<ShelterSubscription> {
    return this.api.post<ShelterSubscription>(
      `${this.endpoint}/${shelterId}/subscribe`,
      null
    );
  }

  deleteShelterSubscription(shelterId: string): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/${shelterId}/subscribe`, '');
  }
}
