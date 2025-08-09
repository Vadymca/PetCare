import { CommonModule } from '@angular/common';
import { Component, computed, inject, Signal } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AuthButtonsComponent } from '../../shared/components/auth-buttons.component';
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
  ],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent {
  private authService = inject(AuthService);
  private translate = inject(TranslateService);
  menuItems: Record<string, string> = {
    '/animals': 'ANIMALS',
    '/shelters': 'SHELTERS',
    '/articles': 'ARTICLES',
    '/lost-pets': 'LOST_PETS',
    '/animal-aid-requests': 'ANIMAL_AID_REQUEST',
  };
  get menuItemKeys(): string[] {
    return Object.keys(this.menuItems);
  }

  router = inject(Router);
  isMenuOpen = false;
  isAuthenticated: Signal<boolean> = this.authService.isLoggedIn;
  userName: Signal<string | null> = computed(() => {
    const user = this.authService.currentUser();
    return user ? user.firstName : null;
  });

  changeLanguage(lang: string) {
    this.translate.use(lang);
  }

  get currentLang(): string {
    return this.translate.currentLang || this.translate.defaultLang;
  }
  logout() {
    this.authService.logout();
  }
}
