import { UpperCasePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { Article } from '../../../core/models/article';

@Component({
  selector: 'app-article-card',
  standalone: true,
  imports: [UpperCasePipe, TranslateModule],
  templateUrl: './article-card.component.html',
  styleUrl: './article-card.component.css',
})
export class ArticleCardComponent {
  @Input() article: Partial<Article> | undefined;
}
