import { AdoptionApplication } from './adoptionApplication';

export interface SuccessStory {
  id: string;
  slug?: string;
  title: string;
  adoptionApplicationId?: string;
  adoptionApplication?: AdoptionApplication;
  photos: string[]; // Масив URL або ідентифікаторів фото
  videos: string[]; // Масив URL або ідентифікаторів відео
  description: string;
  shortDescription?: string;
  createdAt: string;
  updatedAt: string;
}
