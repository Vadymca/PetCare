import { TestBed } from '@angular/core/testing';

import { GuardianshipService } from './guardianship.service';

describe('GuardianshipServiceService', () => {
  let service: GuardianshipService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GuardianshipService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
