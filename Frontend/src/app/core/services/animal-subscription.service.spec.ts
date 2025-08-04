import { TestBed } from '@angular/core/testing';

import { AnimalSubscriptionService } from './animal-subscription.service';

describe('AnimalSubscriptionService', () => {
  let service: AnimalSubscriptionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AnimalSubscriptionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
