import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-user-menu',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule],
  template: `
    <div class="relative">
      <button
        (click)="toggleMenu()"
        class="px-4 py-2 rounded hover:text-orange-300 transition"
      >
        {{ 'HELLO' | translate }}, {{ userName }}!
      </button>

      @if (menuOpen) {
        <ul
          class="absolute right-0 mt-2 w-48 bg-gray-800 text-white border rounded shadow-lg z-10"
        >
          <li>
            <a
              routerLink="/profile"
              class="block px-4 py-2 hover:text-orange-300 transition"
              >{{ 'MY_PROFILE' | translate }}</a
            >
          </li>
          <li>
            <button
              (click)="logout.emit()"
              class="w-full text-left px-4 py-2 hover:text-orange-300 transition"
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
  @Input() userName = '';
  @Output() logout = new EventEmitter<void>();

  menuOpen = false;

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }
}
