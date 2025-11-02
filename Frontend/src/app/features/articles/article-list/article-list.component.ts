import { CommonModule, isPlatformBrowser } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
  PLATFORM_ID,
  signal,
} from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { catchError, filter, of } from 'rxjs';
import { ArticleService } from '../../../core/services/article.service';
import { HomePartnersComponent } from '../../../shared/components/home-partners/home-partners.component';
import { IconComponent } from '../../../shared/components/icon.component';
import { PaginationComponent } from '../../../shared/components/pagination/pagination.component';
import { ArticleCardComponent } from '../article-card/article-card.component';

@Component({
  selector: 'app-article-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule,
    IconComponent,
    ArticleCardComponent,
    HomePartnersComponent,
    PaginationComponent,
  ],
  templateUrl: './article-list.component.html',
  styleUrl: './article-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArticleListComponent {
  router = inject(Router);
  currentPage = signal(2);
  totalPages = signal(10);
	platformId = inject(PLATFORM_ID);
  constructor() {
    effect(() => {
      if (isPlatformBrowser(this.platformId)) {
        this.router.events
          .pipe(filter(event => event instanceof NavigationEnd))
          .subscribe(() => {
            window.scrollTo({ top: 0, behavior: 'auto' });
          });
      }
    });
  }
  setPage(page: number) {
    this.currentPage.set(page);
  }
  backBottomClick() {
    this.router.navigate(['about']);
  }
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
