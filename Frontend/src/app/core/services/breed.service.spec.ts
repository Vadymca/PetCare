import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';

import { Breed } from '../interfaces/breed';
import { BreedService } from './breed.service';

describe('BreedService', () => {
  let service: BreedService;
  let httpClientSpy: jasmine.SpyObj<HttpClient>;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);

    TestBed.configureTestingModule({
      providers: [
        BreedService,
        { provide: HttpClient, useValue: httpClientSpy },
      ],
    });

    service = TestBed.inject(BreedService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getAll', () => {
    it('should return expected breeds array', done => {
      const expectedBreeds: Breed[] = [
        {
          id: '1',
          speciesId: 'sp1',
          name: 'Breed1',
          description: 'Description1',
        },
        {
          id: '2',
          speciesId: 'sp2',
          name: 'Breed2',
          description: 'Description2',
        },
      ];

      httpClientSpy.get.and.returnValue(of(expectedBreeds));

      service.getAll().subscribe({
        next: breeds => {
          expect(breeds).toEqual(expectedBreeds);
          expect(httpClientSpy.get.calls.count()).toBe(1);
          done();
        },
        error: done.fail,
      });
    });

    it('should propagate http error', done => {
      const errorResponse = new Error('Http error');
      httpClientSpy.get.and.returnValue(throwError(() => errorResponse));

      service.getAll().subscribe({
        next: () => done.fail('expected an error'),
        error: err => {
          expect(err).toBe(errorResponse);
          done();
        },
      });
    });
  });

  describe('getBreedById', () => {
    it('should return expected breed by id', done => {
      const expectedBreed: Breed = {
        id: '1',
        speciesId: 'sp1',
        name: 'Breed1',
        description: 'Description1',
      };

      httpClientSpy.get.and.returnValue(of(expectedBreed));

      service.getBreedById('1').subscribe({
        next: breed => {
          expect(breed).toEqual(expectedBreed);
          expect(httpClientSpy.get.calls.count()).toBe(1);
          done();
        },
        error: done.fail,
      });
    });

    it('should propagate http error on getBreedById', done => {
      const errorResponse = new Error('Http error');
      httpClientSpy.get.and.returnValue(throwError(() => errorResponse));

      service.getBreedById('123').subscribe({
        next: () => done.fail('expected an error'),
        error: err => {
          expect(err).toBe(errorResponse);
          done();
        },
      });
    });
  });
});
