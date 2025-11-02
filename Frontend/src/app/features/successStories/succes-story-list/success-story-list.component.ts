import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, effect, inject, PLATFORM_ID, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { catchError, filter, of } from 'rxjs';
import { SuccessStoryService } from '../../../core/services/success-story.service';
import { HomePartnersComponent } from '../../../shared/components/home-partners/home-partners.component';
import { IconComponent } from '../../../shared/components/icon.component';
import { PaginationComponent } from '../../../shared/components/pagination/pagination.component';
import { ArticleCardComponent } from '../../articles/article-card/article-card.component';

@Component({
  selector: 'app-success-storylist',
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
  templateUrl: './success-story-list.component.html',
  styleUrl: './success-story-list.component.css',
})
export class SuccessStoryListComponent {
  router = inject(Router);
  currentPage = signal(2);
  totalPages = signal(10);
  platformId = inject(PLATFORM_ID);
  private successStoryService = inject(SuccessStoryService);
  successStories = toSignal(
    this.successStoryService.getSuccessStories().pipe(
      catchError(err => {
        this.error.set('FAILED_TO_LOAD_SUCCESS_STORIES');
        console.error('Error loading success stories:', err);
        return of([]); // Повертаємо порожній список, щоб Signal не впав
      })
    ),
    { initialValue: [] }
  );
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

  error = signal<string | null>(null);
}
