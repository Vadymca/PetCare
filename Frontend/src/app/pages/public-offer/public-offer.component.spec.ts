import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicOfferComponent } from './public-offer.component';

describe('PublicOfferComponent', () => {
  let component: PublicOfferComponent;
  let fixture: ComponentFixture<PublicOfferComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PublicOfferComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PublicOfferComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
