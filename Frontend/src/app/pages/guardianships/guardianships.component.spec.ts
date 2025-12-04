import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuardianshipsComponent } from './guardianships.component';

describe('GuardianshipsComponent', () => {
  let component: GuardianshipsComponent;
  let fixture: ComponentFixture<GuardianshipsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GuardianshipsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GuardianshipsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
