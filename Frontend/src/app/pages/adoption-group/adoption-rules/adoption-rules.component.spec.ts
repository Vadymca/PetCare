import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdoptionRulesComponent } from './adoption-rules.component';

describe('AdoptionRulesComponent', () => {
  let component: AdoptionRulesComponent;
  let fixture: ComponentFixture<AdoptionRulesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdoptionRulesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdoptionRulesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
