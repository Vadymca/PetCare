import { Breed } from './breed';
import { User } from './user';

export type LostStatus = 'Lost' | 'Found' | 'Reunited' | string;

export interface LostPet {
  id: string;
  slug: string;
  userId: string;
  user?: User;
  breedId: string;
  breed?: Breed;
  name: string;
  description: string;
  lastSeenLocation: string;
  lastSeenDate: string;
  photos: string[];

  status: LostStatus;
  adminNotes: string;
  reward: number;
  contactAlternative: string;
  microchipId: string;
  createdAt: string;
  updatedAt: string;
}
