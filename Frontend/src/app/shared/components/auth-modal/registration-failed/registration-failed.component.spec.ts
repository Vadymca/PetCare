import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationFailedComponent } from './registration-failed.component';

describe('RegistrationFailedComponent', () => {
  let component: RegistrationFailedComponent;
  let fixture: ComponentFixture<RegistrationFailedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrationFailedComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegistrationFailedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
