import { UpperCasePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, effect, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
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
  constructor() {
    effect(() => {
      this.loadRules();

      this.translate.onLangChange.subscribe(() => {
        this.loadRules();
      });
    });
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
