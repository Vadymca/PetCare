import { LowerCasePipe, UpperCasePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
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
  private router = inject(Router);
  news: NewsPreview[] = [
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
  onSeeAllNewsClick() {
    this.router.navigate(['news']);
  }
}
