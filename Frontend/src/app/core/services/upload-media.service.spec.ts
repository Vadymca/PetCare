import { TestBed } from '@angular/core/testing';

import { UploadMediaService } from './upload-media.service';

describe('UploadMediaService', () => {
  let service: UploadMediaService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UploadMediaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
