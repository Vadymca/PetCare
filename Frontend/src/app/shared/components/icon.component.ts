import { Component, Input, effect, inject, signal } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
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
  @Input() name?: keyof typeof ICONS;
  @Input() hostClass = '';
  private sanitizer = inject(DomSanitizer);

  // сигнал для іконки
  svg = signal<SafeHtml | null>(null);

  constructor() {
    // ефект реагує на зміни name
    effect(() => {
      if (this.name && ICONS[this.name]) {
        this.svg.set(this.sanitizer.bypassSecurityTrustHtml(ICONS[this.name]));
      } else {
        this.svg.set(null);
      }
    });
  }
}
