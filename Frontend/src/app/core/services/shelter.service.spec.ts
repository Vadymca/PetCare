import { TestBed } from '@angular/core/testing';
import { ShelterService } from './shelter.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Shelter } from '../interfaces/shelter';
import { API_BASE_URL } from '../config/api.config';

describe('ShelterService', () => {
  let service: ShelterService;
  let httpMock: HttpTestingController;
  const baseUrl = `${API_BASE_URL}/shelters`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ShelterService],
    });

    service = TestBed.inject(ShelterService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('#getShelters', () => {
    it('should return an array of shelters', () => {
      const mockShelters: Shelter[] = [
        { id: '1', slug: 'shelter-1', name: 'Shelter 1', address: 'Addr 1', coordinates: { lat: 1, lng: 1 }, contactPhone: '123', contactEmail: 'a@b.com', description: 'desc', capacity: 10, currentOccupancy: 5, photos: [], virtualTourUrl: null, workingHours: '9-18', socialMedia: {}, managerId: 'mgr1', createdAt: '2023-01-01', updatedAt: '2023-01-01' },
        { id: '2', slug: 'shelter-2', name: 'Shelter 2', address: 'Addr 2', coordinates: { lat: 2, lng: 2 }, contactPhone: '456', contactEmail: 'c@d.com', description: 'desc2', capacity: 20, currentOccupancy: 15, photos: [], virtualTourUrl: null, workingHours: '10-19', socialMedia: {}, managerId: 'mgr2', createdAt: '2023-01-02', updatedAt: '2023-01-02' }
      ];

      service.getShelters().subscribe(shelters => {
        expect(shelters.length).toBe(2);
        expect(shelters).toEqual(mockShelters);
      });

      const req = httpMock.expectOne(baseUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockShelters);
    });

    it('should return an empty array if no shelters found', () => {
      service.getShelters().subscribe(shelters => {
        expect(shelters.length).toBe(0);
      });

      const req = httpMock.expectOne(baseUrl);
      req.flush([]);
    });

    it('should handle HTTP error', () => {
      service.getShelters().subscribe({
        next: () => fail('expected an error'),
        error: error => {
          expect(error.status).toBe(500);
        }
      });

      const req = httpMock.expectOne(baseUrl);
      req.flush('Server error', { status: 500, statusText: 'Internal Server Error' });
    });
  });

  describe('#getShelterBySlug', () => {
    const slug = 'shelter-1';

    it('should return a single shelter matching the slug', () => {
      const mockShelter: Shelter = {
        id: '1', slug, name: 'Shelter 1', address: 'Addr 1', coordinates: { lat: 1, lng: 1 }, contactPhone: '123', contactEmail: 'a@b.com',
        description: 'desc', capacity: 10, currentOccupancy: 5, photos: [], virtualTourUrl: null, workingHours: '9-18',
        socialMedia: {}, managerId: 'mgr1', createdAt: '2023-01-01', updatedAt: '2023-01-01'
      };

      service.getShelterBySlug(slug).subscribe(shelter => {
        expect(shelter).toEqual(mockShelter);
      });

      const req = httpMock.expectOne(`${baseUrl}?slug=${slug}`);
      expect(req.request.method).toBe('GET');
      req.flush([mockShelter]);
    });

    it('should return undefined if no shelter matches the slug', () => {
      service.getShelterBySlug(slug).subscribe(shelter => {
        expect(shelter).toBeUndefined();
      });

      const req = httpMock.expectOne(`${baseUrl}?slug=${slug}`);
      req.flush([]);
    });

    it('should handle HTTP error', () => {
      service.getShelterBySlug(slug).subscribe({
        next: () => fail('expected an error'),
        error: error => {
          expect(error.status).toBe(404);
        }
      });

      const req = httpMock.expectOne(`${baseUrl}?slug=${slug}`);
      req.flush('Not Found', { status: 404, statusText: 'Not Found' });
    });
  });
});
