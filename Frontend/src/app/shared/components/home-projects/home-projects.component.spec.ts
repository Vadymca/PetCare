import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeProjectsComponent } from './home-projects.component';

describe('HomeProjectsComponent', () => {
  let component: HomeProjectsComponent;
  let fixture: ComponentFixture<HomeProjectsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HomeProjectsComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(HomeProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
