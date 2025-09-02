import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { ICONS } from '../../../../../assets/icons/icons';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-secondary-small-orange-button',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule, IconComponent],
  template: `
    <button
      type="button"
      class="flex items-center justify-center gap-2 
			h-10 rounded-40 p-4 uppercase pr-10 pl-10 
			font-inter font-bold text-base 
			bg-primary-light-orange text-primary-beige
		hover:bg-primary-beige hover:text-primary-light-orange 
    active:bg-secondary-treePoppy-2 active:text-primary-light-orange  
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
export class SecondarySmallOrangeButtonComponent {
  @Input() buttonTitle?: string;
  @Input() iconName?: keyof typeof ICONS;
  @Output() pressButton = new EventEmitter<void>();
  @Input() loading? = signal(false);

  @Input() disabled = signal(false); // <- новий Input

  async confirm() {
    if (this.disabled()) return; // додатково блокування
    this.pressButton.emit();
  }
}
