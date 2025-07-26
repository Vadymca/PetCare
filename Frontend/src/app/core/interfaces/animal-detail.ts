import { Breed } from './breed';
import { Shelter } from './shelter';
import { Species } from './species';
import { User } from './user';

export interface AnimalDetail {
  id: string;
  name: string;

  userId?: string;
  breedId?: string;
  shelterId?: string;

  breed?: Breed;
  species?: Species;
  shelter?: Shelter;
  user?: User; // якщо потрібно
  age: [number, number];

  slug: string;
  birthday: string; // ISO дата у форматі рядка
  gender: string;
  description: string;
  healthStatus: string;
  photos: string[]; // Масив URL або ідентифікаторів фото
  videos: string[]; // Масив URL або ідентифікаторів відео

  status: string;
  adoptionRequirements: string;
  microchipId: string;
  idNumber: number;
  weight: number;
  height: number;
  color: string;
  isSterilized: boolean;
  haveDocuments: boolean;
  createdAt: string; // ISO дата рядка
  updatedAt: string; // ISO дата рядка
}
