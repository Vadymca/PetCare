import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../interfaces/user';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private api = inject(ApiService);
  private endpoint = `users`;

  getAll(): Observable<User[]> {
    return this.api.get<User[]>(this.endpoint);
  }

  getUserById(id: string): Observable<User> {
    return this.api.getById<User>(this.endpoint, id);
  }

  createUser(user: Partial<User>): Observable<User> {
    return this.api.post<User>(this.endpoint, user);
  }

  updateUser(id: string, user: Partial<User>): Observable<User> {
    return this.api.put<User>(this.endpoint, id, user);
  }

  deleteUser(id: string): Observable<void> {
    return this.api.delete<void>(this.endpoint, id);
  }
}
