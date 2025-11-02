import { Breed } from './breed';
import { Shelter } from './shelter';
import { Species } from './species';

export interface Animal {
  id: string; //+
  slug: string; //+
  name: string; //+

  userId?: string;
  breedId?: string; //+
  shelterId?: string; //+

  breed?: Breed; //+
  species?: Species; //+
  shelter?: Shelter; //+
  age?: [number, number]; //моє поле!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  //для підписок
  isChecked: boolean; //моє поле!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  isFavorite?: boolean; //моє поле!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  animalSubscriptionId?: string; //моє поле!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

  birthday: string; // ISO дата у форматі рядка//+
  gender: string; //+
  isUnderCare?: boolean; //+
  description: string; //+
  healthConditions: string[]; //+
  specialNeeds: string[]; //+
  temperaments: string[]; //+
  size: string; //+
  careCost?: string; //+
  photo?: string; //+ тільки для списку (для карточок тварин)
  photos: string[]; // Масив URL або ідентифікаторів фото//+
  videos: string[]; // Масив URL або ідентифікаторів відео//+

  status: string; //+
  adoptionRequirements: string; //+??????????????????
  microchipId: string; //+
  weight: number; //+
  height: number; //+
  color: string; //+
  isSterilized: boolean; //+
  haveDocuments: boolean; //+
  createdAt: string; // ISO дата рядка//+
  updatedAt: string; // ISO дата рядка//+
}
export interface AnimalListResult {
  animals: Animal[];
  totalCount: number;
}
