import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  Output,
  WritableSignal,
  signal,
} from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { ICONS } from '../../../../../assets/icons/icons';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-download-orange-button-with-icon-and-border',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule, IconComponent],
  template: `
    <button
      type="button"
      [disabled]="disabledSignal()"
      class="flex items-center justify-center gap-2 
        h-[40px] w-[40px] rounded-[12px] p-4 uppercase  
        font-inter font-bold text-base 
        bg-primary-orange text-primary-beige 
        hover:bg-primary-beige hover:text-primary-orange hover:border-2 hover:border-primary-light-orange 
        active:bg-secondary-treePoppy-1 active:text-primary-orange 
        disabled:bg-secondary-neutral-alto disabled:text-secondary-neutral-doveGray disabled:border-none"
      (click)="confirm()"
    >
      @if (iconName) {
        <app-icon [name]="iconName"></app-icon>
      }
      @if (buttonTitle) {
        <span class="text-base">{{ buttonTitle | translate }}</span>
      }
      <ng-content select="[icon]"></ng-content>
    </button>
  `,
})
export class DownloadOrangeButtonWithIconAndBorderComponent {
  @Input() iconName?: keyof typeof ICONS;
  @Input() buttonTitle?: string;
  @Output() pressButton = new EventEmitter<void>();

  @Input() loading?: WritableSignal<boolean> = signal(false);

  // створюємо WritableSignal за замовчуванням
  disabledSignal: WritableSignal<boolean> = signal(false);

  // дозволяємо передавати свій сигнал, якщо він є
  @Input({ required: false })
  set _disabledSignal(value: WritableSignal<boolean> | undefined) {
    if (value) {
      this.disabledSignal = value;
    }
  }

  async confirm() {
    if (this.disabledSignal()) return;
    this.pressButton.emit();
  }
}
