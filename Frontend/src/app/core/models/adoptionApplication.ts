import { Animal } from './animal';
import { User } from './user';

export interface AdoptionApplication {
  id: string;
  animalId?: string;
  animal?: Animal;
  userId?: string;
  user?: User;
  status: string;
  createdAt: string;
  updatedAt: string;
}
