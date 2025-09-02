import { UpperCasePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, effect, inject } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Router } from 'express';
import { IconComponent } from '../../shared/components/icon.component';

@Component({
  selector: 'app-privacy-policy',
  standalone: true,
  imports: [TranslateModule, IconComponent, UpperCasePipe],
  templateUrl: './privacy-policy.component.html',
  styleUrl: './privacy-policy.component.css',
})
export class PrivacyPolicyComponent {
  translate = inject(TranslateService);
  http = inject(HttpClient);
  private router = inject(Router);
  rulesHtml = '';
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
      .get(`/assets/i18n/rules/${lang}.html`, { responseType: 'text' })
      .subscribe(html => (this.rulesHtml = html));
  }
  backButtonClick() {
    this.router.navigate(['/']);
  }
}
