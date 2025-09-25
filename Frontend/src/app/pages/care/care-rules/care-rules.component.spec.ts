import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CareRulesComponent } from './care-rules.component';

describe('CareRulesComponent', () => {
  let component: CareRulesComponent;
  let fixture: ComponentFixture<CareRulesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CareRulesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CareRulesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
