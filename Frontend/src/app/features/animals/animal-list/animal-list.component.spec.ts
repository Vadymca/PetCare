import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';
import { TranslateModule } from '@ngx-translate/core';
import { of, throwError } from 'rxjs';
import { AnimalDetail } from '../../../core/models/animal-detail';
import { AnimalService } from '../../../core/services/animal.service';
import { AnimalListComponent } from './animal-list.component';

describe('AnimalListComponent', () => {
  let component: AnimalListComponent;
  let fixture: ComponentFixture<AnimalListComponent>;
  let animalServiceSpy: jasmine.SpyObj<AnimalService>;

  const mockAnimals: AnimalDetail[] = [
    {
      id: '1',
      name: 'Барсик',
      slug: 'barsik',
      species: { id: 'sp1', name: 'Кіт' },
      breed: {
        id: 'b1',
        speciesId: 'sp1',
        name: 'Сіамський',
        description: 'Короткошерста порода котів',
      },
      shelter: undefined,
      user: undefined,
      age: [2, 5],
      birthday: '2023-01-01',
      gender: 'male',
      description: '',
      healthStatus: '',
      photos: [],
      videos: [],
      status: '',
      adoptionRequirements: '',
      microchipId: '',
      idNumber: 123,
      weight: 4.5,
      height: 20,
      color: 'сірий',
      isSterilized: true,
      haveDocuments: true,
      createdAt: '',
      updatedAt: '',
    },
  ];

  beforeEach(async () => {
    animalServiceSpy = jasmine.createSpyObj('AnimalService', [
      'getAnimalsWithDetails',
    ]);
    await TestBed.configureTestingModule({
      imports: [
        AnimalListComponent,
        TranslateModule.forRoot(),
        RouterTestingModule,
      ],
      providers: [{ provide: AnimalService, useValue: animalServiceSpy }],
    }).compileComponents();
  });

  it('should create', () => {
    animalServiceSpy.getAnimalsWithDetails.and.returnValue(of(mockAnimals));
    fixture = TestBed.createComponent(AnimalListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  it('should render a list of animals', () => {
    animalServiceSpy.getAnimalsWithDetails.and.returnValue(of(mockAnimals));
    fixture = TestBed.createComponent(AnimalListComponent);
    fixture.detectChanges();

    const cards = fixture.debugElement.queryAll(By.css('a'));
    expect(cards.length).toBe(1);
    const name = cards[0].query(By.css('h2')).nativeElement.textContent;
    expect(name).toContain('Барсик');
  });

  it('should show nothing if animal list is empty', () => {
    animalServiceSpy.getAnimalsWithDetails.and.returnValue(of([]));
    fixture = TestBed.createComponent(AnimalListComponent);
    fixture.detectChanges();

    const cards = fixture.debugElement.queryAll(By.css('a'));
    expect(cards.length).toBe(0);
  });

  it('should handle service error gracefully', () => {
    spyOn(console, 'error'); // suppress error logs
    animalServiceSpy.getAnimalsWithDetails.and.returnValue(
      throwError(() => new Error('Server error'))
    );
    fixture = TestBed.createComponent(AnimalListComponent);
    fixture.detectChanges();

    const cards = fixture.debugElement.queryAll(By.css('a'));
    expect(cards.length).toBe(0);
  });
});
