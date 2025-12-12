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
    const year = this.isCurrentYearSelected()
      ? this.currentYear
      : this.currentYear - 1;

    // Місяць в файлі — це 1-12, а ти передаєш 0-11 (ймовірно з DatePicker), тому +1
    //const monthPadded = (month + 1).toString().padStart(2, '0');
    const monthNumber = month + 1;
    const fileName = `${year}-${monthNumber}.pdf`;
    const specificPath = `../../../assets/files/reports/${fileName}`;
    const fallbackPath = '../../../assets/files/reports/universal.pdf';

    // Спробуємо перевірити, чи існує конкретний файл
    fetch(specificPath, { method: 'HEAD' })
      .then(response => {
        const url = response.ok ? specificPath : fallbackPath;
        window.open(url, '_blank');
      })
      .catch(() => {
        // Якщо fetch впав (наприклад, через CORS або мережеву помилку) — на всяк випадок fallback
        window.open(fallbackPath, '_blank');
      });
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
