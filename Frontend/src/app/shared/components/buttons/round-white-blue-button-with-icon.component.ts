import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { IconComponent } from '../icon.component';

@Component({
  selector: 'app-round-white-blue-button-with-icon',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule, IconComponent],
  template: `
    <button
      type="button"
      class="flex items-center justify-center gap-2 
    h-[40px] w-[40px] rounded-40 p-4 uppercase  
    font-inter font-bold text-base 
    bg-primary-beige text-primary-blue
    hover:bg-primary-beige hover:text-primary-blue 
    active:bg-secondary-chileanFire-1 active:text-primary-blue 
    disabled:bg-secondary-neutral-alto disabled:text-secondary-neutral-doveGray"
      (click)="confirm()"
      (mouseenter)="hovered.set(true)"
      (mouseleave)="hovered.set(false); active.set(false)"
      (mousedown)="active.set(true)"
      (mouseup)="active.set(false)"
      [disabled]="disabled()"
    >
      <app-icon
        [name]="hovered() || active() ? 'heartFilled' : 'heart'"
      ></app-icon>

      <ng-content select="[icon]"></ng-content>
    </button>
  `,
})
export class RoundWhiteBlueButtonWithIconComponent {
  @Output() pressButton = new EventEmitter<void>();

  disabled = signal(false); // <- новий Input
  hovered = signal(false);
  active = signal(false);
  constructor() {
    this.disabled.set(false);
    this.hovered.set(false);
    this.active.set(false);
  }
  async confirm() {
    if (this.disabled()) return; // додатково блокування
    this.pressButton.emit();
  }
}
