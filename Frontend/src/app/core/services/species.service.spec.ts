import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';

import { Species } from '../models/species';
import { SpeciesService } from './species.service';

describe('SpeciesService', () => {
  let service: SpeciesService;
  let httpClientSpy: jasmine.SpyObj<HttpClient>;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);

    TestBed.configureTestingModule({
      providers: [
        SpeciesService,
        { provide: HttpClient, useValue: httpClientSpy },
      ],
    });

    service = TestBed.inject(SpeciesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getAll', () => {
    it('should return expected species array', done => {
      const expectedSpecies: Species[] = [
        { id: '1', name: 'Cat' },
        { id: '2', name: 'Dog' },
      ];

      httpClientSpy.get.and.returnValue(of(expectedSpecies));

      service.getAll().subscribe({
        next: species => {
          expect(species).toEqual(expectedSpecies);
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

  describe('getSpeciesById', () => {
    it('should return expected species by id', done => {
      const expectedSpecies: Species = { id: '1', name: 'Cat' };

      httpClientSpy.get.and.returnValue(of(expectedSpecies));

      service.getSpeciesById('1').subscribe({
        next: species => {
          expect(species).toEqual(expectedSpecies);
          expect(httpClientSpy.get.calls.count()).toBe(1);
          done();
        },
        error: done.fail,
      });
    });

    it('should propagate http error on getSpeciesById', done => {
      const errorResponse = new Error('Http error');
      httpClientSpy.get.and.returnValue(throwError(() => errorResponse));

      service.getSpeciesById('123').subscribe({
        next: () => done.fail('expected an error'),
        error: err => {
          expect(err).toBe(errorResponse);
          done();
        },
      });
    });
  });
});
