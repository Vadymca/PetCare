import { isPlatformBrowser, UpperCasePipe } from '@angular/common';
import { Component, inject, PLATFORM_ID, signal } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { REQUEST_ORIGIN } from '../../../core/tokens/request-origin.token';
import { IconComponent } from '../../../shared/components/icon.component';
import { filter } from 'rxjs';

@Component({
  selector: 'app-care-rules',
  standalone: true,
  imports: [TranslateModule, IconComponent, UpperCasePipe, RouterModule],
  templateUrl: './care-rules.component.html',
  styleUrl: './care-rules.component.css',
})
export class CareRulesComponent {
  private translate = inject(TranslateService);
  private router = inject(Router);
  origin = inject(REQUEST_ORIGIN);
  platformId = inject(PLATFORM_ID);

  // сигнал для мови
  lang = signal(this.translate.currentLang || this.translate.getDefaultLang());

  constructor() {
    // слухаємо зміну мови
    this.translate.onLangChange.subscribe(() => {
      this.lang.set(this.translate.currentLang);
    });
    if (isPlatformBrowser(this.platformId)) {
      this.router.events
        .pipe(filter(event => event instanceof NavigationEnd))
        .subscribe(() => {
          window.scrollTo({ top: 0, behavior: 'auto' });
        });
    }
  }

  backButtonClick() {
    this.router.navigate(['/']);
  }
}
