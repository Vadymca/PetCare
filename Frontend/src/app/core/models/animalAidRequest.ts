import { Shelter } from './shelter';
import { User } from './user';

export type AidCategory = 'Food' | 'Medical' | 'Equipment' | 'Other';
export type AidRequestStatus =
  | 'Open'
  | 'InProgress'
  | 'Fulfilled'
  | 'Cancelled';
export interface AnimalAidRequest {
  id: string;
  userId: string;
  user?: User; // якщо потрібно
  shelterId: string;
  shelter?: Shelter;
  title: string;
  description: string;
  category: AidCategory;
  status: AidRequestStatus;
  estimatedCost: number; // Орієнтовна  вартість
  photos: string[]; // Масив URL або ідентифікаторів фото

  createdAt: string; // ISO дата рядка
  updatedAt: string; // ISO дата рядка
}
