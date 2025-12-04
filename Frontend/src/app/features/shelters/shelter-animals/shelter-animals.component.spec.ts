import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShelterAnimalsComponent } from './shelter-animals.component';

describe('ShelterAnimalsComponent', () => {
  let component: ShelterAnimalsComponent;
  let fixture: ComponentFixture<ShelterAnimalsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShelterAnimalsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShelterAnimalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
