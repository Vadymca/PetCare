import { Shelter } from './shelter';
import { User } from './user';

export interface ShelterSubscription {
  id: string;
  userId: string;
  shelterId: string;

  user?: User;
  shelter?: Shelter;

  subscribedAt: string; // ISO дата рядка
}
