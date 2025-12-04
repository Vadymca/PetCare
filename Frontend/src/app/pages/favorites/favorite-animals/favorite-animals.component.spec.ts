import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FavoriteAnimalsComponent } from './favorite-animals.component';

describe('FavoriteAnimalsComponent', () => {
  let component: FavoriteAnimalsComponent;
  let fixture: ComponentFixture<FavoriteAnimalsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FavoriteAnimalsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FavoriteAnimalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
