import {
  isPlatformBrowser,
  LowerCasePipe,
  UpperCasePipe,
} from '@angular/common';
import { Component, effect, inject, PLATFORM_ID } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { filter } from 'rxjs';
import { Article } from '../../core/models/article';
import { ArticleCardComponent } from '../../features/articles/article-card/article-card.component';
import { SecondaryLargeButtonComponent } from '../../shared/components/buttons/blue/secondary-large-button.component';
import { HomePartnersComponent } from '../../shared/components/home-partners/home-partners.component';
import { IconComponent } from '../../shared/components/icon.component';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    IconComponent,
    LowerCasePipe,
    ArticleCardComponent,
    SecondaryLargeButtonComponent,
    HomePartnersComponent,
  ],
  templateUrl: './about.component.html',
  styleUrl: './about.component.css',
})
export class AboutComponent {
  mission = [
    {
      title: '93',
      content: 'ABOUT_US_MISSION_PARAGRAPH1',
    },
    {
      title: '527',
      content: 'ABOUT_US_MISSION_PARAGRAPH2',
    },
    {
      title: '843',
      content: 'ABOUT_US_MISSION_PARAGRAPH3',
    },
  ];

  histories: Partial<Article>[] = [
    {
      title: 'Історія Сібаса',
      shortContent:
        'Сібас - котик з Херсонської області. Малюка знайшли після обстрілу - він лежав під уламками цегли біля зруйнованого будинку, тремтів і тихо нявчав. Його господаря нестало, і ще кілька днів тваринка не відходила від місця, де востаннє його бачив. Ми витягли його , годували з рук, лікували рани. Спочатку він боявся навіть звуку мотору авто, але з часом почав довіряти, муркотіти та іти на ручки.  Сьогодні Сібас - улюбленець нової родини. Він знову грається, спокійно спить на дивані та вірить людям.',
      image:
        'https://i.pinimg.com/1200x/a4/db/c3/a4dbc3ff7b023b111ea17cd7699a090a.jpg',
    },
    {
      title: 'Історія Міри',
      shortContent:
        'Міру вивезли під обстрілами. Виснажена, замерзша, вся в болоті і колючках, вона стояла в занедбаному сараї на околиці окупованого села, з пораненою лапою, ланцюгом замість повідка і в очах — лише страх. Після тривалого лікування, кількох місяців турботи й відновлення, її фото побачила німецька родина — фермери з невеликого мальовничого селища в Баварії. Вони сказали: «Ми не шукаємо породистої собаки, ми хочемо подарувати дім тій, хто потребує в даний момент його найбільше». Сьогодні Міра в безпеці , в люблячій і турботливій сім’ї .',
      image:
        'https://i.pinimg.com/1200x/ae/cc/69/aecc69202a906dad9a65be6f1fd1dc0d.jpg',
    },
  ];
  router = inject(Router);
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
  onAllStoriesClick() {
    this.router.navigate(['/success-stories']);
  }
}
