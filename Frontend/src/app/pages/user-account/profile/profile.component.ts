import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, effect, inject, PLATFORM_ID, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { filter } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { PrimaryLargeOrangeButtonComponent } from '../../../shared/components/buttons/orange/primary-large-orange-button.component';
import { ConfirmModalComponent } from '../../../shared/components/confirm-modal/confirm-modal.component';
import { IconComponent } from '../../../shared/components/icon.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TranslateModule,
    IconComponent,
    PrimaryLargeOrangeButtonComponent,
    ConfirmModalComponent,
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css',
})
export class ProfileComponent {
  hovered = signal<number | null>(null);

  private authService = inject(AuthService);

  public user = signal(this.authService._currentUser());
  profilePhoto = signal<string | ArrayBuffer | null>(null);
  router = inject(Router);
  platformId = inject(PLATFORM_ID);
  showLogoutModal = signal(false);
  constructor() {
    effect(() => {
      this.user.set(this.authService._currentUser());
      const userValue = this.user();
      if (userValue) {
        this.profilePhoto.set(userValue.profilePhoto || null);
      }
    });
    if (isPlatformBrowser(this.platformId)) {
      this.router.events
        .pipe(filter(event => event instanceof NavigationEnd))
        .subscribe(() => {
          window.scrollTo({ top: 0, behavior: 'auto' });
        });
    }
  }
  onMouseEnter(id: number) {
    this.hovered.set(id);
  }

  onMouseLeave() {
    this.hovered.set(null);
  }
  showLogoutModalWindow() {
    this.showLogoutModal.set(true);
  }
  logout($event: boolean) {
    if ($event) {
      this.authService.logout();
    }
    this.showLogoutModal.set(false);
  }
  toDonations() {
    //дописати компонент мої платежі і мої платіжні підписки
    throw new Error('Method not implemented.');
  }
  toNotifications() {
    //дописати компонент з нотифікаціями
    throw new Error('Method not implemented.');
  }
  toApplications() {
    //дописати компонент заявки на усиновлення і заявки на волонтерство
    throw new Error('Method not implemented.');
  }
  toGuardianships() {
    this.router.navigate(['guardianships']);
  }
  editProfile() {
    this.router.navigate(['profile/edit']);
  }
  toFavorites() {
    this.router.navigate(['favorites']);
  }
}
