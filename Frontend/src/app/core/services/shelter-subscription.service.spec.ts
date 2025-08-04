import { TestBed } from '@angular/core/testing';

import { ShelterSubscriptionService } from './shelter-subscription.service';

describe('ShelterSubscriptionService', () => {
  let service: ShelterSubscriptionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ShelterSubscriptionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
