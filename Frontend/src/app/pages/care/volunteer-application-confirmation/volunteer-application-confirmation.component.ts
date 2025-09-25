import { isPlatformBrowser } from '@angular/common';
import { Component, inject, PLATFORM_ID } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { filter } from 'rxjs';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { IconComponent } from '../../../shared/components/icon.component';

@Component({
  selector: 'app-volunteer-application-confirmation',
  standalone: true,
  imports: [IconComponent, TranslateModule, PrimaryLargeButtonComponent],
  templateUrl: './volunteer-application-confirmation.component.html',
  styleUrl: './volunteer-application-confirmation.component.css',
})
export class VolunteerApplicationConfirmationComponent {
  router = inject(Router);
  goSupport() {
    this.router.navigate(['support']);
  }
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
}
