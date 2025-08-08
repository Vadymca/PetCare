import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';

import { User } from '../models/user';
import { UserService } from './user.service';

describe('UserService', () => {
  let service: UserService;
  let httpClientSpy: jasmine.SpyObj<HttpClient>;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);

    TestBed.configureTestingModule({
      providers: [
        UserService,
        { provide: HttpClient, useValue: httpClientSpy },
      ],
    });

    service = TestBed.inject(UserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getAll', () => {
    it('should return expected users (called once)', done => {
      const expectedUsers: User[] = [
        {
          id: '1',
          email: 'test1@example.com',
          passwordHash: 'hash1',
          firstName: 'John',
          lastName: 'Doe',
          phone: '1234567890',
          role: 'User',
          
        },
        {
          id: '2',
          email: 'test2@example.com',
          passwordHash: 'hash2',
          firstName: 'Jane',
          lastName: 'Smith',
          phone: '0987654321',
          role: 'Admin',
          
        },
      ];

      httpClientSpy.get.and.returnValue(of(expectedUsers));

      service.getAll().subscribe({
        next: users => {
          expect(users).toEqual(expectedUsers);
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

  describe('getUserById', () => {
    it('should return expected user by id', done => {
      const expectedUser: User = {
        id: '1',
        email: 'test1@example.com',
        passwordHash: 'hash1',
        firstName: 'John',
        lastName: 'Doe',
        phone: '1234567890',
        role: 'User',
        preferences: { language: 'en' },
      };

      httpClientSpy.get.and.returnValue(of(expectedUser));

      service.getUserById('1').subscribe({
        next: user => {
          expect(user).toEqual(expectedUser);
          expect(httpClientSpy.get.calls.count()).toBe(1);
          done();
        },
        error: done.fail,
      });
    });

    it('should propagate http error on getUserById', done => {
      const errorResponse = new Error('Http error');
      httpClientSpy.get.and.returnValue(throwError(() => errorResponse));

      service.getUserById('123').subscribe({
        next: () => done.fail('expected an error'),
        error: err => {
          expect(err).toBe(errorResponse);
          done();
        },
      });
    });
  });
});
