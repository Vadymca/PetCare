import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExistingEmailErrorComponent } from './existing-email-error.component';

describe('ExistingEmailErrorComponent', () => {
  let component: ExistingEmailErrorComponent;
  let fixture: ComponentFixture<ExistingEmailErrorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExistingEmailErrorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExistingEmailErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
