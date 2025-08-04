import { Animal } from './animal';
import { User } from './user';

export interface AnimalSubscription {
  id: string;
  userId: string;
  animalId: string;

  user?: User;
  animal?: Animal;

  subscribedAt: string; // ISO дата рядка
}
