import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { AnimalService } from './animal.service';
import { BreedService } from './breed.service';
import { ShelterService } from './shelter.service';
import { SpeciesService } from './species.service';
import { UserService } from './user.service';

describe('AnimalService', () => {
  let service: AnimalService;
  let httpClientSpy: jasmine.SpyObj<HttpClient>;
  let breedServiceSpy: jasmine.SpyObj<BreedService>;
  let shelterServiceSpy: jasmine.SpyObj<ShelterService>;
  let speciesServiceSpy: jasmine.SpyObj<SpeciesService>;
  let userServiceSpy: jasmine.SpyObj<UserService>;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    breedServiceSpy = jasmine.createSpyObj('BreedService', ['getBreedById']);
    shelterServiceSpy = jasmine.createSpyObj('ShelterService', [
      'getShelterById',
    ]);
    speciesServiceSpy = jasmine.createSpyObj('SpeciesService', [
      'getSpeciesById',
    ]);
    userServiceSpy = jasmine.createSpyObj('UserService', ['getUserById']);

    TestBed.configureTestingModule({
      providers: [
        AnimalService,
        { provide: HttpClient, useValue: httpClientSpy },
        { provide: BreedService, useValue: breedServiceSpy },
        { provide: ShelterService, useValue: shelterServiceSpy },
        { provide: SpeciesService, useValue: speciesServiceSpy },
        { provide: UserService, useValue: userServiceSpy },
      ],
    });

    service = TestBed.inject(AnimalService);
  });

  describe('getAnimalDetailBySlug', () => {
    it('should return animal detail with all related data', done => {
      const slug = 'test-slug';
      const animal = {
        id: '1',
        slug,
        breedId: 'b1',
        shelterId: 's1',
        userId: 'u1',
        birthday: '2020-01-01',
      };
      const breed = {
        id: 'b1',
        speciesId: 'sp1',
        name: 'BreedName',
        description: 'desc',
      };
      const shelter = { id: 's1', name: 'ShelterName' } as any;
      const user = { id: 'u1', firstName: 'John' } as any;
      const species = { id: 'sp1', name: 'SpeciesName' };

      httpClientSpy.get.and.returnValue(of([animal]));
      breedServiceSpy.getBreedById.and.returnValue(of(breed));
      shelterServiceSpy.getShelterById.and.returnValue(of(shelter));
      userServiceSpy.getUserById.and.returnValue(of(user));
      speciesServiceSpy.getSpeciesById.and.returnValue(of(species));

      service.getAnimalBySlug(slug).subscribe(result => {
        expect(result).toBeTruthy();
        expect(result?.breed).toEqual(breed);
        expect(result?.shelter).toEqual(shelter);
        expect(result?.user).toEqual(user);
        expect(result?.species).toEqual(species);
        expect(result?.age[0]).toBeGreaterThanOrEqual(3); // since birthday 2020-01-01 and now 2025
        done();
      });
    });

    it('should return undefined if no animal found', done => {
      httpClientSpy.get.and.returnValue(of([])); // empty array means no animal with slug

      service.getAnimalBySlug('not-exist').subscribe(result => {
        expect(result).toBeUndefined();
        done();
      });
    });

    it('should handle missing breed and return undefined', done => {
      const animal = {
        id: '1',
        slug: 'slug',
        breedId: 'b1',
        birthday: '2020-01-01',
      };
      httpClientSpy.get.and.returnValue(of([animal]));
      breedServiceSpy.getBreedById.and.returnValue(of(undefined)); // breed missing

      service.getAnimalBySlug('slug').subscribe(result => {
        expect(result).toBeUndefined();
        done();
      });
    });

    it('should handle http error gracefully', done => {
      httpClientSpy.get.and.returnValue(
        throwError(() => new Error('Http error'))
      );

      service.getAnimalBySlug('slug').subscribe({
        next: () => fail('expected error'),
        error: err => {
          expect(err).toBeTruthy();
          done();
        },
      });
    });
  });

  describe('getAnimalsWithDetails', () => {
    it('should return array of animal details', done => {
      const animals = [
        {
          id: '1',
          breedId: 'b1',
          shelterId: 's1',
          userId: 'u1',
          birthday: '2020-01-01',
        },
        { id: '2', breedId: 'b2', birthday: '2019-05-10' }, // no shelterId, no userId
      ];
      const breed1 = {
        id: 'b1',
        speciesId: 'sp1',
        name: 'Breed1',
        description: 'desc',
      };
      const breed2 = {
        id: 'b2',
        speciesId: 'sp2',
        name: 'Breed2',
        description: 'desc',
      };
      const shelter = { id: 's1', name: 'Shelter1' } as any;
      const user = { id: 'u1', firstName: 'User1' } as any;
      const species1 = { id: 'sp1', name: 'Species1' };
      const species2 = { id: 'sp2', name: 'Species2' };

      httpClientSpy.get.and.returnValue(of(animals));
      breedServiceSpy.getBreedById.withArgs('b1').and.returnValue(of(breed1));
      breedServiceSpy.getBreedById.withArgs('b2').and.returnValue(of(breed2));
      shelterServiceSpy.getShelterById.and.returnValue(of(shelter));
      userServiceSpy.getUserById.and.returnValue(of(user));
      speciesServiceSpy.getSpeciesById
        .withArgs('sp1')
        .and.returnValue(of(species1));
      speciesServiceSpy.getSpeciesById
        .withArgs('sp2')
        .and.returnValue(of(species2));

      service.getAnimals().subscribe(result => {
        expect(result.length).toBe(2);
        expect(result[0].breed).toEqual(breed1);
        expect(result[0].shelter).toEqual(shelter);
        expect(result[0].user).toEqual(user);
        expect(result[0].species).toEqual(species1);

        expect(result[1].breed).toEqual(breed2);
        expect(result[1].shelter).toBeUndefined();
        expect(result[1].user).toBeUndefined();
        expect(result[1].species).toEqual(species2);
        done();
      });
    });

    it('should filter out animals with missing breed', done => {
      const animals = [
        { id: '1', breedId: 'b1', birthday: '2020-01-01' },
        { id: '2', breedId: 'b2', birthday: '2019-05-10' },
      ];
      httpClientSpy.get.and.returnValue(of(animals));
      breedServiceSpy.getBreedById
        .withArgs('b1')
        .and.returnValue(of(undefined)); // no breed
      breedServiceSpy.getBreedById.withArgs('b2').and.returnValue(
        of({
          id: 'b2',
          speciesId: 'sp2',
          name: 'Breed2',
          description: 'desc',
        })
      );
      speciesServiceSpy.getSpeciesById.and.returnValue(
        of({ id: 'sp2', name: 'Species2' })
      );

      service.getAnimals().subscribe(result => {
        expect(result.length).toBe(1);
        expect(result[0].breed?.id).toBe('b2');
        done();
      });
    });

    it('should handle empty animal list', done => {
      httpClientSpy.get.and.returnValue(of([]));

      service.getAnimals().subscribe(result => {
        expect(result.length).toBe(0);
        done();
      });
    });

    it('should handle http error in getAnimals', done => {
      httpClientSpy.get.and.returnValue(
        throwError(() => new Error('Http error'))
      );

      service.getAnimals().subscribe({
        next: () => fail('expected error'),
        error: err => {
          expect(err).toBeTruthy();
          done();
        },
      });
    });
  });
});
