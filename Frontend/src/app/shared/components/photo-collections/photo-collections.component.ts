import { CommonModule, isPlatformBrowser } from '@angular/common';
import {
  Component,
  inject,
  Input,
  OnInit,
  PLATFORM_ID,
  signal,
} from '@angular/core';
import { IconComponent } from '../icon.component';

@Component({
  selector: 'app-photo-collections',
  standalone: true,
  imports: [IconComponent, CommonModule],
  templateUrl: './photo-collections.component.html',
  styleUrl: './photo-collections.component.css',
})
export class PhotoCollectionsComponent implements OnInit {
  @Input({ required: true }) photos: string[] = [];
  items = signal<string[]>([]);
  visibleCount = signal(3);
  offset = signal(0);
  animating = signal(false);
  platformId = inject(PLATFORM_ID);
  selectedItem = signal<string>('');
  fadeIn = signal(true);
  isVisible = signal(true); // Додаємо для керування visibility

  constructor() {
    // Ініціалізація items із photos або дефолтними значеннями

    console.log(this.photos);

    // Встановлюємо початкове зображення
    this.selectedItem.set(this.items()[0] || '');
  }
  ngOnInit() {
    // Ініціалізація items із photos або дефолтними значеннями

    this.items.set([...this.photos]);

    this.selectedItem.set(this.items()[0] || '');
  }

  selectItem(img: string) {
    if (this.animating() || img === this.selectedItem()) {
      console.log('Blocked: animating or same image', {
        img,
        current: this.selectedItem(),
      });
      return;
    }

    console.log('SelectItem:', {
      newImg: img,
      current: this.selectedItem(),
      fadeIn: this.fadeIn(),
      isVisible: this.isVisible(),
    });

    this.animating.set(true);
    this.fadeIn.set(false);
    this.isVisible.set(false); // Приховуємо зображення

    if (isPlatformBrowser(this.platformId)) {
      setTimeout(() => {
        this.selectedItem.set(img);
        requestAnimationFrame(() => {
          setTimeout(() => {
            this.isVisible.set(true); // Показуємо нове зображення
            this.fadeIn.set(true);
            console.log('FadeIn and isVisible set to true:', {
              selected: this.selectedItem(),
              fadeIn: this.fadeIn(),
              isVisible: this.isVisible(),
            });
            setTimeout(() => {
              this.animating.set(false);
              console.log('Cleanup:', {
                selected: this.selectedItem(),
                fadeIn: this.fadeIn(),
                isVisible: this.isVisible(),
              });
            }, 700); // Чекаємо 700ms анімації + запас
          }, 100); // Затримка для рендерингу
        });
      }, 200); // Збільшено для повного приховування
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

    // переміщаємо останню картку на початок масиву
    const arr = [...this.items()];
    const last = arr.pop();
    if (last) arr.unshift(last);
    this.items.set(arr);

    this.offset.set(-100 / this.visibleCount());

    // одразу зсуваємо на -1 картку
    this.offset.set(-100 / this.visibleCount());

    // невелика затримка, щоб анімація спрацювала
    setTimeout(() => {
      this.animating.set(true);
      this.offset.set(0); // плавний рух вправо на 1 картку
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
