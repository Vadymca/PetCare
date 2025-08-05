import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LostPetsListComponent } from './lost-pets-list.component';

describe('LostPetsListComponent', () => {
  let component: LostPetsListComponent;
  let fixture: ComponentFixture<LostPetsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LostPetsListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LostPetsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
