import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnimalAidRequestDetailComponent } from './animal-aid-request-detail.component';

describe('AnimalAidRequestDetailComponent', () => {
  let component: AnimalAidRequestDetailComponent;
  let fixture: ComponentFixture<AnimalAidRequestDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AnimalAidRequestDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AnimalAidRequestDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
