import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuardianshipCardComponent } from './guardianship-card.component';

describe('GuardianshipCardComponent', () => {
  let component: GuardianshipCardComponent;
  let fixture: ComponentFixture<GuardianshipCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GuardianshipCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GuardianshipCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
