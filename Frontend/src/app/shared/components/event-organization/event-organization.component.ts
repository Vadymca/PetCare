import { HttpClient } from '@angular/common/http';
import { Component, effect, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-event-organization',
  standalone: true,
  imports: [TranslateModule],
  templateUrl: './event-organization.component.html',
  styleUrl: './event-organization.component.css',
})
export class EventOrganizationComponent {
  translate = inject(TranslateService);
  http = inject(HttpClient);
  private router = inject(Router);
  eventsHtml = '';
  constructor() {
    effect(() => {
      this.loadPage();

      this.translate.onLangChange.subscribe(() => {
        this.loadPage();
      });
    });
  }
  loadPage() {
    const lang = this.translate.currentLang || this.translate.getDefaultLang();
    this.http
      .get(`/assets/i18n/events/${lang}.html`, { responseType: 'text' })
      .subscribe(html => (this.eventsHtml = html));
  }
}
