import { UpperCasePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { SuccessStory } from '../../../core/models/successStory';

@Component({
  selector: 'app-succes-story-card',
  standalone: true,
  imports: [UpperCasePipe, TranslateModule],
  templateUrl: './success-story-card.component.html',
  styleUrl: './success-story-card.component.css',
})
export class SuccessStoryCardComponent {
  @Input() successStory: Partial<SuccessStory> | undefined;
}
