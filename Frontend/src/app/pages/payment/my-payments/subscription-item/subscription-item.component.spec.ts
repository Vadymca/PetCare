import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubscriptionItemComponent } from './subscription-item.component';

describe('SubscriptionItemComponent', () => {
  let component: SubscriptionItemComponent;
  let fixture: ComponentFixture<SubscriptionItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubscriptionItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SubscriptionItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
