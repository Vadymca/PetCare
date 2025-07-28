import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ShelterListComponent } from './shelter-list.component';
import { ShelterService } from '../../../core/services/shelter.service';
import { of, throwError } from 'rxjs';
import { By } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

describe('ShelterListComponent', () => {
  let component: ShelterListComponent;
  let fixture: ComponentFixture<ShelterListComponent>;
  let shelterServiceMock: jasmine.SpyObj<ShelterService>;

  const mockShelters = [
    {
      id: '1',
      slug: 'shelter-1',
      name: 'Shelter One',
      address: 'Address 1',
      contactPhone: '123456789',
    },
    {
      id: '2',
      slug: 'shelter-2',
      name: 'Shelter Two',
      address: 'Address 2',
      contactPhone: '987654321',
    },
  ];

  beforeEach(async () => {
    shelterServiceMock = jasmine.createSpyObj('ShelterService', ['getShelters']);

    await TestBed.configureTestingModule({
      imports: [ShelterListComponent, CommonModule, RouterTestingModule, TranslateModule.forRoot()],
      providers: [{ provide: ShelterService, useValue: shelterServiceMock }],
    }).compileComponents();
  });

  function setupShelterResponse(response$: any) {
    shelterServiceMock.getShelters.and.returnValue(response$);
    fixture = TestBed.createComponent(ShelterListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }

  it('should create', () => {
    setupShelterResponse(of(mockShelters));
    expect(component).toBeTruthy();
  });

  it('should render a list of shelters', () => {
    setupShelterResponse(of(mockShelters));
    const shelterElements = fixture.debugElement.queryAll(By.css('a'));
    expect(shelterElements.length).toBe(mockShelters.length);
    expect(shelterElements[0].nativeElement.textContent).toContain('Shelter One');
    expect(shelterElements[1].nativeElement.textContent).toContain('Shelter Two');
  });

  it('should render empty state if no shelters are returned', () => {
    setupShelterResponse(of([]));
    const shelterElements = fixture.debugElement.queryAll(By.css('a'));
    expect(shelterElements.length).toBe(0);
    // Optionally check for an empty message in the template if present
  });

  it('should handle errors from the service gracefully', () => {
    setupShelterResponse(throwError(() => new Error('Service error')));
    const shelterElements = fixture.debugElement.queryAll(By.css('a'));
    expect(shelterElements.length).toBe(0);
    // You can check for an error UI element if your template displays one
  });
});
