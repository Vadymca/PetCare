export interface TeamMember {
  id: string; //+
  name: string; //+
  linkedInUrl?: string;
  role?: string;
  videoUrl?: string;
  isFavorite: boolean;
}
