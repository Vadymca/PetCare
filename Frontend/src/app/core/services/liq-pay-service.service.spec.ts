import { TestBed } from '@angular/core/testing';

import { LiqPayServiceService } from './liq-pay-service.service';

describe('LiqPayServiceService', () => {
  let service: LiqPayServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LiqPayServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
