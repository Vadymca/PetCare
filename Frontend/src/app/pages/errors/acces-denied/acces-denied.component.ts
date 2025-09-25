import { Component, inject, PLATFORM_ID } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { IconComponent } from '../../../shared/components/icon.component';
import { isPlatformBrowser } from '@angular/common';
import { filter } from 'rxjs';

@Component({
  selector: 'app-acces-denied',
  standalone: true,
  imports: [TranslateModule, PrimaryLargeButtonComponent, IconComponent],

  templateUrl: './acces-denied.component.html',
  styleUrl: './acces-denied.component.css',
})
export class AccesDeniedComponent {
  router = inject(Router);
  platformId = inject(PLATFORM_ID);
  constructor() {
    if (isPlatformBrowser(this.platformId)) {
      this.router.events
        .pipe(filter(event => event instanceof NavigationEnd))
        .subscribe(() => {
          window.scrollTo({ top: 0, behavior: 'auto' });
        });
    }
  }
  goHome() {
    this.router.navigate(['/']);
  }
}
