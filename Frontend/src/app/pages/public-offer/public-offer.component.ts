import { isPlatformBrowser, UpperCasePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, effect, inject, PLATFORM_ID } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs';
import { REQUEST_ORIGIN } from '../../core/tokens/request-origin.token';
import { IconComponent } from '../../shared/components/icon.component';

@Component({
  selector: 'app-public-offer',
  standalone: true,
  imports: [TranslateModule, IconComponent, UpperCasePipe, RouterModule],
  templateUrl: './public-offer.component.html',
  styleUrl: './public-offer.component.css',
})
export class PublicOfferComponent {
  translate = inject(TranslateService);
  http = inject(HttpClient);
  private router = inject(Router);
  origin = inject(REQUEST_ORIGIN);
  platformId = inject(PLATFORM_ID);
  siteName = this.origin;

  publicOfferHtml = '';

  constructor() {
    effect(() => {
      this.loadPublicOffer();

      this.translate.onLangChange.subscribe(() => {
        this.loadPublicOffer();
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
  loadPublicOffer() {
    const lang = this.translate.currentLang || this.translate.getDefaultLang();
    this.http
      .get(`/assets/i18n/publicOffer/${lang}.html`, { responseType: 'text' })
      .subscribe(html => {
        // замінюємо плейсхолдер {{siteName}} на реальне значення
        this.publicOfferHtml = html.replace(/{{\s*siteName\s*}}/g, this.origin);
      });
  }
  backButtonClick() {
    this.router.navigate(['/']);
  }
}
