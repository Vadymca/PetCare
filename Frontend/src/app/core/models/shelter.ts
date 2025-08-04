import { User } from './user';

export interface Coordinates {
  lat: number;
  lng: number;
}

export type SocialMedia = Record<string, string>;

export interface Shelter {
  id: string;
  slug: string;
  name: string;
  address: string;
  coordinates: Coordinates;
  contactPhone: string;
  contactEmail: string;
  description: string;
  capacity: number;
  currentOccupancy: number;
  photos: string[]; // масив URL або шляхів до фото
  virtualTourUrl: string | null;
  workingHours: string;
  socialMedia: SocialMedia;
  managerId: string;
  manager?: User;
  createdAt: string; // ISO 8601
  updatedAt: string;
}
