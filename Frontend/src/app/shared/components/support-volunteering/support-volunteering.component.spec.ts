import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportVolunteeringComponent } from './support-volunteering.component';

describe('SupportVolunteeringComponent', () => {
  let component: SupportVolunteeringComponent;
  let fixture: ComponentFixture<SupportVolunteeringComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SupportVolunteeringComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SupportVolunteeringComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
