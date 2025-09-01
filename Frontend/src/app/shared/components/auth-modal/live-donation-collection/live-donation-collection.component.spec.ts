import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LiveDonationCollectionComponent } from './live-donation-collection.component';

describe('LiveDonationCollectionComponent', () => {
  let component: LiveDonationCollectionComponent;
  let fixture: ComponentFixture<LiveDonationCollectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LiveDonationCollectionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LiveDonationCollectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
