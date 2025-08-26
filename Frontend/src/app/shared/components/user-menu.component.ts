import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  EventEmitter,
  inject,
  Output,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { filter, fromEvent } from 'rxjs';
import { RoundButtonWithIconComponent } from './buttons/round-button-with-icon.component';

@Component({
  selector: 'app-user-menu',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule,
    RoundButtonWithIconComponent,
  ],
  template: `
    <div #menuWrapper class="relative">
      <app-round-button-with-icon
        class="text-primary-beige"
        [iconName]="'userRound'"
        (click)="toggleMenu()"
      ></app-round-button-with-icon>
      <!-- <button
        (click)="toggleMenu()"
        class="px-4 py-2 rounded hover:text-orange-300 transition"
      >
        {{ 'HELLO' | translate }}, {{ userName }}!
      </button> -->

      @if (menuOpen) {
        <ul
          class="absolute right-0 mt-2 w-48 bg-primary-blue text-secondary-neutral-white rounded shadow-lg z-10"
        >
          <li>
            <a
              routerLink="/profile"
              (click)="menuOpen = false"
              class="block px-4 py-2 hover:text-primary-orange hover:underline transition"
              >{{ 'MY_PROFILE' | translate }}</a
            >
          </li>
          <li>
            <button
              (click)="logout.emit()"
              class="w-full text-left px-4 py-2 hover:text-primary-orange hover:underline transition"
            >
              {{ 'LOGOUT' | translate }}
            </button>
          </li>
        </ul>
      }
    </div>
  `,
})
export class UserMenuComponent {
  // @Input() userName = '';
  @Output() logout = new EventEmitter<void>();

  menuOpen = false;
  private elementRef = inject(ElementRef);

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }
  // Хост-лістенер кліків по документу
  constructor() {
    // Підписка на кліки по документу
    fromEvent<MouseEvent>(document, 'click')
      .pipe(
        takeUntilDestroyed(),
        filter(event => {
          const wrapper =
            this.elementRef.nativeElement.querySelector('#menuWrapper') ||
            this.elementRef.nativeElement;
          return !wrapper.contains(event.target as Node) && this.menuOpen;
        })
      )
      .subscribe(() => (this.menuOpen = false));
  }
}
