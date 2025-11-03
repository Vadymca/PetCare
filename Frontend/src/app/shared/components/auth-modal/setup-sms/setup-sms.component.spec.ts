import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetupSmsComponent } from './setup-sms.component';

describe('SetupSmsComponent', () => {
  let component: SetupSmsComponent;
  let fixture: ComponentFixture<SetupSmsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SetupSmsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SetupSmsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
