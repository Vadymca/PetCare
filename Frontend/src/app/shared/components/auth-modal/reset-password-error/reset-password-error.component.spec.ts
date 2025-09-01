import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetPasswordErrorComponent } from './reset-password-error.component';

describe('ResetPasswordErrorComponent', () => {
  let component: ResetPasswordErrorComponent;
  let fixture: ComponentFixture<ResetPasswordErrorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ResetPasswordErrorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ResetPasswordErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
