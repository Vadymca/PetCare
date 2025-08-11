import { TestBed } from '@angular/core/testing';

import { AnimalAidRequestService } from './animal-aid-request.service';

describe('AnimalAidRequestService', () => {
  let service: AnimalAidRequestService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AnimalAidRequestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
