import { isPlatformBrowser, UpperCasePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, effect, inject, PLATFORM_ID } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs';
import { IconComponent } from '../../shared/components/icon.component';

@Component({
  selector: 'app-terms-and-conditions',
  standalone: true,
  imports: [TranslateModule, IconComponent, UpperCasePipe],
  templateUrl: './terms-and-conditions.component.html',
  styleUrl: './terms-and-conditions.component.css',
})
export class TermsAndConditionsComponent {
  translate = inject(TranslateService);
  http = inject(HttpClient);
  private router = inject(Router);
  termsAndConditionsHtml = '';

  platformId = inject(PLATFORM_ID);
  constructor() {
    effect(() => {
      this.loadRules();

      this.translate.onLangChange.subscribe(() => {
        this.loadRules();
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
  loadRules() {
    const lang = this.translate.currentLang || this.translate.getDefaultLang();
    this.http
      .get(`/assets/i18n/termsAndConditionsHtml/${lang}.html`, {
        responseType: 'text',
      })
      .subscribe(html => (this.termsAndConditionsHtml = html));
  }
  backButtonClick() {
    this.router.navigate(['/']);
  }
}
