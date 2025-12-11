import {
  isPlatformBrowser,
  LowerCasePipe,
  UpperCasePipe,
} from '@angular/common';
import {
  Component,
  effect,
  inject,
  Input,
  PLATFORM_ID,
  signal,
} from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs';
import { NewsPreview } from '../../../core/models/newsPreview';
import { SecondaryLargeButtonComponent } from '../buttons/blue/secondary-large-button.component';
import { IconComponent } from '../icon.component';

@Component({
  selector: 'app-home-news',
  standalone: true,
  imports: [
    TranslateModule,
    SecondaryLargeButtonComponent,
    IconComponent,
    LowerCasePipe,
    UpperCasePipe,
  ],
  templateUrl: './home-news.component.html',
  styleUrl: './home-news.component.css',
})
export class HomeNewsComponent {
  @Input() toList = false;

  private router = inject(Router);

  translate = inject(TranslateService);
  platformId = inject(PLATFORM_ID);
  news = signal<NewsPreview[]>([]);
  newsUk: NewsPreview[] = [
    {
      id: '1',
      title: 'фест',
      content:
        'ТУТ ЗУСТРІЧАЮТЬСЯ ТІ, ХТО ШУКАЄ ДІМ, І ТІ , ХТО ГОТОВИЙ ПОДАРУВАТИ ЛЮБОВ І ТЕПЛО',
    },
    {
      id: '2',
      title: 'вдома',
      content:
        'ДВАДЦЯТЬ ТВАРИНОК З НАШОГО ПРИТУЛКУ УСПІШНО ЗНАЙШЛИ ДІМ ЗА ОСТАННІЙ МІСЯЦЬ',
    },
    {
      id: '3',
      title: 'порятунок',
      content: 'ПОТЯТУНОК ХВОСТИКІВ У НАДСКЛАДНИХ УМОВАХ ВІЙНИ',
    },
  ];
  newsEn: NewsPreview[] = [
    {
      id: '1',
      title: 'Festival',
      content:
        'THIS IS WHERE THOSE WHO ARE LOOKING FOR A HOME MEET THOSE WHO ARE READY TO GIVE LOVE AND WARMTH',
    },
    {
      id: '2',
      title: 'Home at Last',
      content:
        'TWENTY ANIMALS FROM OUR SHELTER HAVE SUCCESSFULLY FOUND A HOME IN THE LAST MONTH',
    },
    {
      id: '3',
      title: 'Rescue',
      content: 'SAVING TAILS IN EXTREMELY CHALLENGING WAR CONDITIONS',
    },
  ];
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
      this.news.set(this.newsEn);
    } else if (lang === 'uk') {
      this.news.set(this.newsUk);
    }
  }

  onSeeAllNewsClick() {
    if (this.toList) this.router.navigate(['latest-news']);
    else this.router.navigate(['news']);
  }
}
