import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  inject,
  Input,
  Output,
  signal,
} from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ICONS } from '../../../../../assets/icons/icons';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-primary-large-button',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule, IconComponent],
  template: `
    <button
      type="button"
      class="flex items-center justify-center gap-2 w-full
        h-[52px] rounded-40 p-4 uppercase pr-10 pl-10 
        font-bold text-base 
        bg-primary-blue text-primary-beige 
        hover:bg-secondary-dodgerBlue-2
        active:bg-secondary-dodgerBlue-3 
        disabled:bg-secondary-neutral-alto disabled:text-secondary-neutral-doveGray"
      (click)="confirm()"
      [disabled]="disabled()"
      [ngClass]="{ 'cursor-progress': loading?.() }"
    >
      @if (iconName) {
        <app-icon [name]="iconName"></app-icon>
      }

      <ng-content select="[icon]"></ng-content>
      <span>{{ buttonTitle ?? '' | translate | uppercase }}</span>
    </button>
  `,
})
export class PrimaryLargeButtonComponent {
  router = inject(Router);
  translate = inject(TranslateService);
  @Input() buttonTitle: string | undefined;
  @Input() iconName?: keyof typeof ICONS;
  @Output() pressButton = new EventEmitter<void>();
  @Input() loading? = signal(false);

  @Input() disabled = signal(false); // <- новий Input

  async confirm() {
    if (this.disabled()) return; // додатково блокування
    this.pressButton.emit();
  }
}
