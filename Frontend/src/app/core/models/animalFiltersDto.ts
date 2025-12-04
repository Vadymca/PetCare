export interface AnimalFiltersDto {
  page?: number;
  pageSize?: number;
  genders?: string[]; // ["male"]
  sizes?: string[]; // ["small"]
  statuses?: string[]; // ["available"]
  isSterilized?: boolean;
  isUndercare?: boolean;
  minAge?: number;
  maxAge?: number;
  careCosts?: string[]; // ["sixHundred"]
  animalTypeFilter?: string; // "cats" | "dogs" | "others"
  shelterId?: string;
  specieId?: string;
  breedId?: string;
  search?: string;
}
