import { TestBed } from '@angular/core/testing';

import { SuccessStoryService } from './success-story.service';

describe('SuccessStoryService', () => {
  let service: SuccessStoryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SuccessStoryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
