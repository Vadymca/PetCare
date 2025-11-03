import { CommonModule, isPlatformBrowser } from '@angular/common';
import {
  Component,
  inject,
  Input,
  OnChanges,
  PLATFORM_ID,
  signal,
  SimpleChanges,
} from '@angular/core';
import { IconComponent } from '../icon.component';

@Component({
  selector: 'app-photo-collections',
  standalone: true,
  imports: [IconComponent, CommonModule],
  templateUrl: './photo-collections.component.html',
  styleUrl: './photo-collections.component.css',
})
export class PhotoCollectionsComponent implements OnChanges {
  @Input({ required: true }) photos: string[] = [];
  items = signal<string[]>([]);
  visibleCount = signal(3);
  offset = signal(0);
  animating = signal(false);
  platformId = inject(PLATFORM_ID);
  selectedItem = signal<string>('');
  fadeIn = signal(true);
  isVisible = signal(true);

  constructor() {
    this.items.set([...this.photos]);
    this.selectedItem.set(this.items()[0] || '');
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['photos']) {
      this.items.set([...this.photos]);
      this.selectedItem.set(this.items()[0] || '');
    }
  }

  selectItem(img: string) {
    if (this.animating() || img === this.selectedItem()) return;

    this.animating.set(true);
    this.fadeIn.set(false);
    this.isVisible.set(false);

    if (isPlatformBrowser(this.platformId)) {
      setTimeout(() => {
        this.selectedItem.set(img);
        requestAnimationFrame(() => {
          setTimeout(() => {
            this.isVisible.set(true);
            this.fadeIn.set(true);
            setTimeout(() => {
              this.animating.set(false);
            }, 700);
          }, 100);
        });
      }, 200);
    } else {
      this.selectedItem.set(img);
      this.fadeIn.set(true);
      this.isVisible.set(true);
      this.animating.set(false);
    }
  }

  next() {
    if (this.animating()) return;
    this.animating.set(true);
    this.offset.set(-100 / this.visibleCount());
  }

  prev() {
    if (this.animating()) return;
    const arr = [...this.items()];
    const last = arr.pop();
    if (last) arr.unshift(last);
    this.items.set(arr);
    this.offset.set(-100 / this.visibleCount());
    setTimeout(() => {
      this.animating.set(true);
      this.offset.set(0);
    }, 10);
  }

  onTransitionEnd() {
    if (this.offset() !== 0) {
      this.items.update(arr => {
        const newArr = [...arr];
        if (this.offset() < 0) {
          const first = newArr.shift();
          return first ? [...newArr, first] : newArr;
        } else if (this.offset() > 0) {
          const last = newArr.pop();
          return last ? [last, ...newArr] : newArr;
        }
        return newArr;
      });
    }

    this.animating.set(false);
    this.offset.set(0);
  }
}
