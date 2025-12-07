import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Guardianship } from '../models/guardianship';
import { GuardianshipCancelationResponse } from '../models/guardianshipCancelationResponse';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root',
})
export class GuardianshipService {
  private api = inject(ApiService);
  private endpoint = `guardianships`;

  createGuardianship(animalId: string): Observable<Guardianship> {
    return this.api.post<Guardianship>(this.endpoint, { animalId: animalId });
  }
  getGuardianships(): Observable<Guardianship[]> {
    return this.api
      .get<Guardianship[]>(`${this.endpoint}/me`)
      .pipe(map(g => g.filter(data => data.status !== 'Completed')));
  }
  cancelGuardianship(guardianshipId: string) {
    return this.api.delete<GuardianshipCancelationResponse>(
      `${this.endpoint}`,
      guardianshipId
    );
  }
}
