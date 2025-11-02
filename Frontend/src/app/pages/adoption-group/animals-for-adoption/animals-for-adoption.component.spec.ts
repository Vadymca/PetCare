import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnimalsForAdoptionComponent } from './animals-for-adoption.component';

describe('AnimalsForAdoptionComponent', () => {
  let component: AnimalsForAdoptionComponent;
  let fixture: ComponentFixture<AnimalsForAdoptionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AnimalsForAdoptionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AnimalsForAdoptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
