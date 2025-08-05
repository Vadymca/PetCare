import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
} from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { catchError, of } from 'rxjs';
import { ArticleService } from '../../../core/services/article.service';

@Component({
  selector: 'app-article-list',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule],
  templateUrl: './article-list.component.html',
  styleUrl: './article-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArticleListComponent {
  private articleService = inject(ArticleService);

  error = signal<string | null>(null);

  articles = toSignal(
    this.articleService.getArticles().pipe(
      catchError(err => {
        this.error.set('FAILED_TO_LOAD_ARTICLES');
        console.error('Error loading articles:', err);
        return of([]); // Повертаємо порожній список, щоб Signal не впав
      })
    ),
    { initialValue: [] }
  );
}
