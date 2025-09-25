import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerApplicationConfirmationComponent } from './volunteer-application-confirmation.component';

describe('VolunteerApplicationConfirmationComponent', () => {
  let component: VolunteerApplicationConfirmationComponent;
  let fixture: ComponentFixture<VolunteerApplicationConfirmationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VolunteerApplicationConfirmationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VolunteerApplicationConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
