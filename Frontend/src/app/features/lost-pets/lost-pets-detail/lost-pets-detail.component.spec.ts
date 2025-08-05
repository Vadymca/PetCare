import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LostPetsDetailComponent } from './lost-pets-detail.component';

describe('LostPetsDetailComponent', () => {
  let component: LostPetsDetailComponent;
  let fixture: ComponentFixture<LostPetsDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LostPetsDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LostPetsDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
