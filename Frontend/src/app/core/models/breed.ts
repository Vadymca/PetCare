import { Species } from './species';

export interface Breed {
  id: string;
  speciesId: string;
  species?: Species;
  name: string;
  description: string;
}
