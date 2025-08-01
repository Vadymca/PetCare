import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { Animal } from '../models/animal';
import { ApiService } from './api.service';

describe('ApiService', () => {
  let service: ApiService;
  let httpMock: HttpTestingController;

  const mockAnimal: Animal = {
    id: '1',
    name: 'Buddy',
    slug: 'buddy',
    birthday: '2020-01-01',
    gender: 'Male',
    description: 'Friendly dog',
    healthStatus: 'Healthy',
    photos: [],
    videos: [],
    status: 'Available',
    adoptionRequirements: 'Love and care',
    microchipId: 'MC123',
    idNumber: 101,
    weight: 20,
    height: 50,
    color: 'Brown',
    isSterilized: true,
    haveDocuments: true,
    createdAt: '2024-01-01T00:00:00Z',
    updatedAt: '2024-01-01T00:00:00Z',
    userId: 'user-1',
    breedId: 'breed-1',
    shelterId: 'shelter-1',
    species: undefined,
    breed: undefined,
    shelter: undefined,
    age: 4,
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ApiService],
    });

    service = TestBed.inject(ApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); // Ensure no open requests
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  // --- GET all animals ---
  it('should GET data from endpoint', () => {
    service.get<Animal[]>('animals').subscribe(data => {
      expect(data.length).toBe(1);
      expect(data[0].name).toBe('Buddy');
    });

    const req = httpMock.expectOne('http://localhost:3000/animals');
    expect(req.request.method).toBe('GET');
    req.flush([mockAnimal]);
  });

  it('should return empty array if no animals found', () => {
    service.get<Animal[]>('animals').subscribe(data => {
      expect(data.length).toBe(0);
    });

    const req = httpMock.expectOne('http://localhost:3000/animals');
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });

  it('should handle error when GET animals', () => {
    service.get<Animal[]>('animals').subscribe({
      next: () => fail('should have failed with 500 error'),
      error: error => {
        expect(error.status).toBe(500);
      },
    });

    const req = httpMock.expectOne('http://localhost:3000/animals');
    expect(req.request.method).toBe('GET');
    req.flush('Server error', { status: 500, statusText: 'Server Error' });
  });

  // --- GET by ID ---
  it('should GET by ID', () => {
    service.getById<Animal>('animals', '1').subscribe(data => {
      expect(data.id).toBe('1');
    });

    const req = httpMock.expectOne('http://localhost:3000/animals/1');
    expect(req.request.method).toBe('GET');
    req.flush(mockAnimal);
  });

  it('should handle 404 error when GET by ID', () => {
    service.getById<Animal>('animals', '999').subscribe({
      next: () => fail('should have failed with 404 error'),
      error: error => {
        expect(error.status).toBe(404);
      },
    });

    const req = httpMock.expectOne('http://localhost:3000/animals/999');
    expect(req.request.method).toBe('GET');
    req.flush('Not Found', { status: 404, statusText: 'Not Found' });
  });

  // --- GET by slug ---
  it('should GET by slug', () => {
    service.getBySlug<Animal>('animals', 'buddy').subscribe(data => {
      expect(data[0].slug).toBe('buddy');
    });

    const req = httpMock.expectOne('http://localhost:3000/animals?slug=buddy');
    expect(req.request.method).toBe('GET');
    req.flush([mockAnimal]);
  });

  it('should return empty array when GET by slug returns nothing', () => {
    service.getBySlug<Animal>('animals', 'unknown-slug').subscribe(data => {
      expect(data.length).toBe(0);
    });

    const req = httpMock.expectOne(
      'http://localhost:3000/animals?slug=unknown-slug'
    );
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });

  it('should handle error when GET by slug', () => {
    service.getBySlug<Animal>('animals', 'buddy').subscribe({
      next: () => fail('should have failed with 500 error'),
      error: error => {
        expect(error.status).toBe(500);
      },
    });

    const req = httpMock.expectOne('http://localhost:3000/animals?slug=buddy');
    expect(req.request.method).toBe('GET');
    req.flush('Server error', { status: 500, statusText: 'Server Error' });
  });
});
