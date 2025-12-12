import {
  CommonModule,
  isPlatformBrowser,
  UpperCasePipe,
} from '@angular/common';
import {
  Component,
  effect,
  inject,
  OnInit,
  PLATFORM_ID,
  signal,
} from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs';
import { News } from '../../../core/models/news';
import { MetaSsrService } from '../../../core/services/meta-ssr.service';
import { NewsService } from '../../../core/services/news.service';
import { IconComponent } from '../../../shared/components/icon.component';
import { PhotoCollectionsComponent } from '../../../shared/components/photo-collections/photo-collections.component';
import { ShareComponent } from '../../../shared/components/share/share.component';

@Component({
  selector: 'app-news-detail',
  standalone: true,
  imports: [
    ShareComponent,
    IconComponent,
    TranslateModule,
    UpperCasePipe,
    CommonModule,
    PhotoCollectionsComponent,
  ],
  templateUrl: './news-detail.component.html',
  styleUrl: './news-detail.component.css',
})
export class NewsDetailComponent implements OnInit {
  translate = inject(TranslateService);
  router = inject(Router);
  private route = inject(ActivatedRoute);
  platformId = inject(PLATFORM_ID);
  newsService = inject(NewsService);
  news = signal<News | null>(null);
  id = signal<string | null>(null);
  textForShare = signal<string>('');
  private metaSsr = inject(MetaSsrService);
  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const newId = params.get('id');
      if (!newId) return;
      this.id.set(newId);
      this.loadNews();
    });
  }
  constructor() {
    effect(() => {
      const idValue = this.id();
      if (!idValue) return;
      this.loadNews();
    });
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
  backBottomClick() {
    this.router.navigate(['latest-news']);
  }
  loadNews() {
    const newsId = this.id();
    if (!newsId) return;

    const lang = this.translate.currentLang || this.translate.getDefaultLang();
    let currentNews: News | null;

    if (lang === 'en') {
      currentNews = this.newsService.getEnNewsById(newsId);
    } else {
      currentNews = this.newsService.getUkNewsById(newsId);
    }

    this.news.set(currentNews || null);
    this.textForShare.set(currentNews?.title ?? '');

    // НОВІ МЕТА-ТЕГИ — заміна всього старого коду
    if (currentNews) {
      this.updateMetaTags(currentNews);
    }
  }
  private updateMetaTags(newsItem: News) {
    const title = `${newsItem.title} — Добродій`;
    const description =
      newsItem.descriptionFirstPart ||
      newsItem.subTitle ||
      'Останні новини притулку Добродій';

    const image =
      newsItem.photos?.[0] ||
      'https://i.pinimg.com/1200x/10/00/98/100098f29ed2970c6f26be5daac404d0.jpg';

    const url = `https://dobrodii.onrender.com/news/${newsItem.id}`;

    this.metaSsr.update(title, description, image, url);
  }
}
