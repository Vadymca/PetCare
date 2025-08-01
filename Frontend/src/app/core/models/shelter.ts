export interface Coordinates {
  lat: number;
  lng: number;
}

export interface SocialMedia {
  [platform: string]: string; // Наприклад: facebook: "https://facebook.com/shelter"
}

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
  createdAt: string; // ISO 8601
  updatedAt: string;
}
