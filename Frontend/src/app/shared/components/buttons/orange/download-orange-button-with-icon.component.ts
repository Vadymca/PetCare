import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { ICONS } from '../../../../../assets/icons/icons';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-download-orange-button-with-icon',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule, IconComponent],
  template: `
    <button
      type="button"
      class="flex items-center justify-center gap-2 
			h-[40px] w-[40px] rounded-[12px] p-4 uppercase  
			font-inter font-bold text-base 
    bg-primary-orange text-primary-beige
		hover:bg-primary-beige hover:text-primary-orange h 
    active:bg-secondary-treePoppy-1 active:text-primary-orange 
     disabled:bg-secondary-neutral-alto disabled:text-secondary-neutral-doveGray"
      (click)="confirm()"
      [disabled]="disabled()"
    >
      @if (iconName) {
        <app-icon [name]="iconName"></app-icon>
      }
      @if (buttonTitle) {
        <span class="text-base">{{ buttonTitle | translate }}</span>
      }
      <ng-content select="[icon]"></ng-content>
      @if (disabled()) {
        @if (loading?.() || false) {
          <span
            class=" animate-spin w-4 h-4 border-2 border-primary-beige border-t-transparent rounded-full"
          ></span>
        }
      }
    </button>
  `,
})
export class DownloadOrangeButtonWithIconComponent {
  @Input() iconName?: keyof typeof ICONS;
  @Input() buttonTitle?: string;
  @Output() pressButton = new EventEmitter<void>();
  @Input() loading? = signal(false);

  @Input() disabled = signal(false); // <- новий Input

  async confirm() {
    if (this.disabled()) return; // додатково блокування
    this.pressButton.emit();
  }
}
