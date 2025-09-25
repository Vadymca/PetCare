import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, inject, PLATFORM_ID, signal } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { filter } from 'rxjs';
import { DownloadOrangeButtonWithIconAndBorderComponent } from '../../shared/components/buttons/orange/download-orange-button-with-icon-and-border.component';
import { IconComponent } from '../../shared/components/icon.component';
@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    IconComponent,

    DownloadOrangeButtonWithIconAndBorderComponent,
  ],
  templateUrl: './reports.component.html',
  styleUrl: './reports.component.css',
})
export class ReportsComponent {
  hoveredYear: 'current' | 'last' | null = null;
  currentYear = new Date().getFullYear();
  currentMonth = new Date().getMonth();

  isCurrentYearSelected = signal(true);
  buttonsDisabled = Array.from({ length: 12 }, (_, i) =>
    signal(this.isDisabled(i + 1))
  );
  tryDisable = signal(true);
  //мок дата з пдф звітами за місяцями

  reports = [
    { month: 1, year: 2024, link: 'тут буде посилання на звіт за січень 2024' },
    { month: 2, year: 2024, link: 'тут буде посилання на звіт за лютий 2024' },
    {
      month: 3,
      year: 2024,
      link: 'тут буде посилання на звіт за березень 2024',
    },
    {
      month: 4,
      year: 2024,
      link: 'тут буде посилання на звіт за квітень 2024',
    },
    {
      month: 5,
      year: 2024,
      link: 'тут буде посилання на звіт за травень 2024',
    },
    {
      month: 6,
      year: 2024,
      link: 'тут буде посилання на звіт за червень 2024',
    },
    { month: 7, year: 2024, link: 'тут буде посилання на звіт за липень 2024' },
    {
      month: 8,
      year: 2024,
      link: 'тут буде посилання на звіт за серпень 2024',
    },
    {
      month: 9,
      year: 2024,
      link: 'тут буде посилання на звіт за вересень 2024',
    },
    {
      month: 10,
      year: 2024,
      link: 'тут буде посилання на звіт за жовтень 2024',
    },
    {
      month: 11,
      year: 2024,
      link: 'тут буде посилання на звіт за листопад 2024',
    },
    {
      month: 12,
      year: 2024,
      link: 'тут буде посилання на звіт за грудень 2024',
    },
    { month: 1, year: 2025, link: 'тут буде посилання на звіт за січень 2025' },
    { month: 2, year: 2025, link: 'тут буде посилання на звіт за лютий 2025' },
    {
      month: 3,
      year: 2025,
      link: 'тут буде посилання на звіт за березень 2025',
    },
    {
      month: 4,
      year: 2025,
      link: 'тут буде посилання на звіт за квітень 2025',
    },
    {
      month: 5,
      year: 2025,
      link: 'тут буде посилання на звіт за травень 2025',
    },
    {
      month: 6,
      year: 2025,
      link: 'тут буде посилання на звіт за червень 2025',
    },
    { month: 7, year: 2025, link: 'тут буде посилання на звіт за липень 2025' },
    {
      month: 8,
      year: 2025,
      link: 'тут буде посилання на звіт за серпень 2025',
    },
  ];
  router = inject(Router);
  platformId = inject(PLATFORM_ID);

  constructor() {
    // ініціалізація сигналу вибору року
    this.isCurrentYearSelected.set(true);
    this.tryDisable.set(true);
    // ініціалізація масиву сигналів для кнопок
    this.buttonsDisabled = Array.from({ length: 12 }, (_, i) =>
      signal(this.isDisabled(i))
    );
    if (isPlatformBrowser(this.platformId)) {
      this.router.events
        .pipe(filter(event => event instanceof NavigationEnd))
        .subscribe(() => {
          window.scrollTo({ top: 0, behavior: 'auto' });
        });
    }
  }
  updateButtonsDisabled() {
    this.buttonsDisabled.forEach((btn, index) => {
      btn.set(this.isDisabled(index));
    });
  }
  onDownloadMonthlyReportClick(month: number) {
    let link = '';
    const year = this.isCurrentYearSelected()
      ? this.currentYear
      : this.currentYear - 1;
    this.reports.forEach(report => {
      if (report.month - 1 === month && report.year === year) {
        link = report.link;
      }
    });
    window.open(link, '_blank');
  }
  isDisabled(month: number) {
    if (!this.isCurrentYearSelected()) {
      return false;
    }
    if (this.currentMonth <= month) {
      return true;
    }
    return false;
  }
  onHover(year: 'current' | 'last') {
    this.hoveredYear = year;
  }

  onLeave() {
    this.hoveredYear = null;
  }

  isCurrentYear() {
    return this.isCurrentYearSelected();
  }
  onLastYearClick() {
    this.isCurrentYearSelected.set(false);
    this.updateButtonsDisabled();
  }
  onCurrentYearClick() {
    this.isCurrentYearSelected.set(true);
    this.updateButtonsDisabled();
  }
}
