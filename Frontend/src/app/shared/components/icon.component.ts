import { Component, Input, effect, inject, signal } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { TranslateService } from '@ngx-translate/core';
import ICONS from '../../../assets/icons/icons';

@Component({
  selector: 'app-icon',
  standalone: true,
  template: `<span [innerHTML]="svg()"></span>`,
  host: {
    '[class]': 'hostClass',
  },
})
export class IconComponent {
  signalName = signal<keyof typeof ICONS | undefined>(undefined);
  @Input() set name(value: keyof typeof ICONS | undefined) {
    this.signalName.set(value);
  }

  @Input() hostClass = '';
  private sanitizer = inject(DomSanitizer);
  private translate = inject(TranslateService);

  // сигнал для іконки
  svg = signal<SafeHtml | null>(null);

  constructor() {
    effect(() => {
      this.translate.onLangChange.subscribe(() => {
        if (this.signalName() === 'logoUkr' && ICONS[this.signalName()!]) {
          if (this.translate.currentLang === 'en') {
            this.svg.set(
              this.sanitizer.bypassSecurityTrustHtml(ICONS['logoEn'])
            );
          } else {
            this.svg.set(
              this.sanitizer.bypassSecurityTrustHtml(ICONS[this.signalName()!])
            );
          }
        }
      });
    });
    // ефект реагує на зміни name
    effect(() => {
      const iconName = this.signalName() || '';
      if (iconName && ICONS[iconName]) {
        if (iconName === 'logoUkr' && this.translate.currentLang === 'en') {
          this.svg.set(this.sanitizer.bypassSecurityTrustHtml(ICONS['logoEn']));
        } else {
          this.svg.set(this.sanitizer.bypassSecurityTrustHtml(ICONS[iconName]));
        }
      } else {
        this.svg.set(null);
      }
    });
  }
}
