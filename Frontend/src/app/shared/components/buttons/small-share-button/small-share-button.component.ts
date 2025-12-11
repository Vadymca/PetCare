import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { IconComponent } from '../../icon.component';
type IconName = 'shareInsta' | 'shareFacebook' | 'shareX' | 'shareTiktok';
@Component({
  selector: 'app-small-share-button',
  imports: [IconComponent],
  templateUrl: './small-share-button.component.html',
  styleUrl: './small-share-button.component.css',
})
export class SmallShareButtonComponent {
  @Output() pressButton = new EventEmitter<void>();

  @Input() iconName = signal<IconName>('shareInsta');

  disabled = signal(false); // <- новий Input
  hovered = signal(false);
  active = signal(false);
  constructor() {
    this.hovered.set(false);
    this.active.set(false);
    this.disabled.set(false);
  }
  async confirm() {
    if (this.disabled()) return; // додатково блокування
    this.pressButton.emit();
  }
}
