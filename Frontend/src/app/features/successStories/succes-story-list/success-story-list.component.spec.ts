import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuccessStoryListComponent } from './success-story-list.component';

describe('SuccessStoryListComponent', () => {
  let component: SuccessStoryListComponent;
  let fixture: ComponentFixture<SuccessStoryListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SuccessStoryListComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SuccessStoryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
