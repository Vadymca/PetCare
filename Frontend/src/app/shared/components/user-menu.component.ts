import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  Output,
  signal,
  ViewChild,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { filter, fromEvent } from 'rxjs';
import { RoundButtonWithIconComponent } from './buttons/round-button-with-icon.component';
import { ConfirmModalComponent } from './confirm-modal/confirm-modal.component';

@Component({
  selector: 'app-user-menu',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule,
    RoundButtonWithIconComponent,
    ConfirmModalComponent,
  ],
  template: `
    <div #menuWrapper class="relative">
      <app-round-button-with-icon
        class="text-primary-beige"
        [iconName]="'userRound'"
        (pressButton)="toggleMenu()"
      ></app-round-button-with-icon>

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
              (click)="showLogoutModalWindow()"
              class="w-full text-left px-4 py-2 hover:text-primary-orange hover:underline transition"
            >
              {{ 'LOGOUT' | translate }}
            </button>
          </li>
        </ul>
      }
    </div>

    @if (showLogoutModal()) {
      <app-confirm-modal
        [text]="'ARE_YOU_SURE_YOU_WANT_TO_EXIT'"
        [titleCancel]="'STAY'"
        [titleSubmit]="'LOGOUT'"
        (confirmAction)="logoutEmit($event)"
      ></app-confirm-modal>
    }
  `,
})
export class UserMenuComponent implements AfterViewInit {
  @Output() logout = new EventEmitter<void>();

  showLogoutModal = signal(false);
  menuOpen = false;
  @ViewChild('menuWrapper', { read: ElementRef }) menuWrapper!: ElementRef;

  ngAfterViewInit() {
    fromEvent<MouseEvent>(document, 'click')
      .pipe(
        takeUntilDestroyed(),
        filter(event => {
          if (!this.menuOpen) return false;
          return !this.menuWrapper.nativeElement.contains(event.target as Node);
        })
      )
      .subscribe(() => (this.menuOpen = false));
  }

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }

  showLogoutModalWindow() {
    this.showLogoutModal.set(true);
  }

  logoutEmit($event: boolean) {
    if ($event) {
      this.logout.emit();
    }
    this.showLogoutModal.set(false);
  }
}
