import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FinancialSupportComponent } from './financial-support.component';

describe('FinancialSupportComponent', () => {
  let component: FinancialSupportComponent;
  let fixture: ComponentFixture<FinancialSupportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FinancialSupportComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FinancialSupportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
