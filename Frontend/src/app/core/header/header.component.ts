import { CommonModule, isPlatformBrowser } from '@angular/common';
import {
  Component,
  computed,
  ElementRef,
  HostListener,
  inject,
  PLATFORM_ID,
  signal,
  Signal,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { filter, fromEvent } from 'rxjs';
import { AuthButtonsComponent } from '../../shared/components/auth-buttons.component';
import { RoundButtonWithIconComponent } from '../../shared/components/buttons/round-button-with-icon.component';
import { IconComponent } from '../../shared/components/icon.component';
import { UserMenuComponent } from '../../shared/components/user-menu.component';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule,
    AuthButtonsComponent,
    UserMenuComponent,
    RoundButtonWithIconComponent,
    IconComponent,
  ],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent {
  openFavourite() {
    throw new Error('Method not implemented.');
  }
  openNotification() {
    throw new Error('Method not implemented.');
  }
  openSearch() {
    throw new Error('Method not implemented.');
  }
  private authService = inject(AuthService);
  private translate = inject(TranslateService);
  private elementRef = inject(ElementRef);
  private platformId = inject(PLATFORM_ID);
  private el = inject(ElementRef);

  menuItems: Record<string, string> = {
    '/animals': 'ANIMALS',
    '/articles': 'ARTICLES',
    '/lost-pets': 'LOST_PETS',
    '/animal-aid-requests': 'ANIMAL_AID_REQUEST',
    '/support': 'SUPPORT',
    '/reports': 'REPORTS',
    '/contacts': 'CONTACTS',
  };
  isHidden = signal(false);
  isFloating = signal(false);
  get menuItemKeys(): string[] {
    return Object.keys(this.menuItems);
  }

  router = inject(Router);
  isMenuOpen = false;
  private lastScrollTop = 0;
  isAuthenticated: Signal<boolean> = this.authService.isLoggedIn;
  userName: Signal<string | null> = computed(() => {
    const user = this.authService._currentUser();
    return user ? user.firstName : null;
  });
  accumulatedDelta = 0;
  @HostListener('window:scroll', ['$event'])
  // onWindowScroll() {
  //   if (isPlatformBrowser(this.platformId)) {
  //     const currentScrollTop =
  //       window.pageYOffset || document.documentElement.scrollTop;

  //     if (currentScrollTop <= 0) {
  //       this.isHidden.set(false);
  //       this.isFloating.set(false);
  //     } else if (currentScrollTop > this.lastScrollTop) {
  //       this.isHidden.set(true);
  //       this.isFloating.set(false);
  //     } else {
  //       this.isFloating.set(true);
  //       this.isHidden.set(false);
  //     }

  //     this.lastScrollTop = currentScrollTop <= 0 ? 0 : currentScrollTop;
  //   }
  // }
  onWindowScroll() {
    const currentScrollTop =
      window.pageYOffset || document.documentElement.scrollTop;
    const delta = currentScrollTop - this.lastScrollTop;

    this.accumulatedDelta += delta;

    const threshold = 10; // наприклад 10px

    if (this.accumulatedDelta > threshold) {
      // скрол вниз
      this.isHidden.set(true);
      this.isFloating.set(false);
      this.accumulatedDelta = 0;
    } else if (this.accumulatedDelta < -threshold) {
      // скрол вгору
      this.isFloating.set(true);
      this.isHidden.set(false);
      this.accumulatedDelta = 0;
    }

    this.lastScrollTop = currentScrollTop <= 0 ? 0 : currentScrollTop;
  }
  changeLanguage(lang: string) {
    this.translate.use(lang);
  }

  get currentLang(): string {
    return this.translate.currentLang || this.translate.defaultLang;
  }
  logout() {
    this.authService.logout();
  }
  constructor() {
    // Підписка на кліки по документу
    if (isPlatformBrowser(this.platformId)) {
      fromEvent<MouseEvent>(document, 'click')
        .pipe(
          takeUntilDestroyed(),
          filter(event => {
            const wrapper =
              this.elementRef.nativeElement.querySelector('#menuWrapper') ||
              this.elementRef.nativeElement;
            return !wrapper.contains(event.target as Node) && this.isMenuOpen;
          })
        )
        .subscribe(() => (this.isMenuOpen = false));
    }
  }
}
