import { isPlatformBrowser, UpperCasePipe } from '@angular/common';
import { Component, effect, inject, PLATFORM_ID, signal } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs';
import { News } from '../../../core/models/news';
import { NewsService } from '../../../core/services/news.service';
import { IconComponent } from '../../../shared/components/icon.component';
import { NewsCardComponent } from '../news-card/news-card.component';
import { ShareComponent } from "../../../shared/components/share/share.component";

@Component({
  selector: 'app-latest-news',
  standalone: true,
  imports: [IconComponent, TranslateModule, UpperCasePipe, NewsCardComponent, ShareComponent],
  templateUrl: './latest-news.component.html',
  styleUrl: './latest-news.component.css',
})
export class LatestNewsComponent {
  translate = inject(TranslateService);
  router = inject(Router);
  platformId = inject(PLATFORM_ID);
  newsService = inject(NewsService);
  news = signal<News[]>([]);
  constructor() {
    effect(() => {
      this.loadNews();

      this.translate.onLangChange.subscribe(() => {
        this.loadNews();
      });
    });
    if (isPlatformBrowser(this.platformId)) {
      this.router.events
        .pipe(filter(event => event instanceof NavigationEnd))
        .subscribe(() => {
          window.scrollTo({ top: 0, behavior: 'auto' });
        });
    }
  }
  loadNews() {
    const lang = this.translate.currentLang || this.translate.getDefaultLang();
    if (lang === 'en') {
      this.news.set(this.newsService.getEnNews());
    } else if (lang === 'uk') {
      this.news.set(this.newsService.getUkNews());
    }
  }
  backBottomClick() {
    this.router.navigate(['news']);
  }
  toNewsClick(id: string) {
    this.router.navigate(['news', id]);
  }
}
