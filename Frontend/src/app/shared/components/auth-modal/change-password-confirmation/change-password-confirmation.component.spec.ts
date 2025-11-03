import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangePasswordConfirmationComponent } from './change-password-confirmation.component';

describe('ChangePasswordConfirmationComponent', () => {
  let component: ChangePasswordConfirmationComponent;
  let fixture: ComponentFixture<ChangePasswordConfirmationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChangePasswordConfirmationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChangePasswordConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
