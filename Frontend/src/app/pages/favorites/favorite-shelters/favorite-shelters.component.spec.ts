import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FavoriteSheltersComponent } from './favorite-shelters.component';

describe('FavoriteSheltersComponent', () => {
  let component: FavoriteSheltersComponent;
  let fixture: ComponentFixture<FavoriteSheltersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FavoriteSheltersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FavoriteSheltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
