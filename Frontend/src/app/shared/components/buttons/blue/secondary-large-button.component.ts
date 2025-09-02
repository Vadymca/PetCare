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
import ICONS from '../../../../../assets/icons/icons';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-secondary-large-button',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule, IconComponent],
  template: `
    <button
      type="button"
      class="flex items-center justify-center gap-2 w-full
			h-[52px] rounded-40 p-4 uppercase pr-10 pl-10 
			font-inter font-bold text-base 
			 bg-primary-lightBlue text-primary-beige
			 hover:bg-secondary-jordyBlue-2 hover:text-primary-blue
			 active:bg-secondary-jordyBlue-3 active:text-primary-beige
			 disabled:bg-secondary-neutral-alto disabled:text-secondary-neutral-doveGray"
      (click)="confirm()"
      [disabled]="disabled()"
    >
      @if (iconName) {
        <app-icon [name]="iconName"></app-icon>
      }

      <ng-content select="[icon]"></ng-content>
      @if (disabled()) {
        @if (loading?.() || false) {
          <span
            class="animate-spin w-4 h-4 border-2 border-primary-beige border-t-transparent rounded-full"
          ></span>
        }
      }

      <span>{{ buttonTitle ?? '' | translate }}</span>
    </button>
  `,
})
export class SecondaryLargeButtonComponent {
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
