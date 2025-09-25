import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportVolunteeringFormComponent } from './support-volunteering-form.component';

describe('SupportVolunteeringFormComponent', () => {
  let component: SupportVolunteeringFormComponent;
  let fixture: ComponentFixture<SupportVolunteeringFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SupportVolunteeringFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SupportVolunteeringFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
