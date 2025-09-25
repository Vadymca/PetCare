import { isPlatformBrowser } from '@angular/common';
import { Component, inject, PLATFORM_ID } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { filter } from 'rxjs';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { IconComponent } from '../../../shared/components/icon.component';

@Component({
  selector: 'app-internal-server-error',
  standalone: true,
  imports: [TranslateModule, PrimaryLargeButtonComponent, IconComponent],
  templateUrl: './internal-server-error.component.html',
  styleUrl: './internal-server-error.component.css',
})
export class InternalServerErrorComponent {
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
