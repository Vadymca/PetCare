import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuccesStoryDetailComponent } from './succes-story-detail.component';

describe('SuccesStoryDetailComponent', () => {
  let component: SuccesStoryDetailComponent;
  let fixture: ComponentFixture<SuccesStoryDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SuccesStoryDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuccesStoryDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
