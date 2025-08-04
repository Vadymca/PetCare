import { TestBed } from '@angular/core/testing';

import { LostPetService } from './lost-pet.service';

describe('LostPetService', () => {
  let service: LostPetService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LostPetService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
