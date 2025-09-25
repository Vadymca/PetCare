import { isPlatformBrowser } from '@angular/common';
import { Component, inject, PLATFORM_ID } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';

@Component({
  selector: 'app-care-subscription-succes',
  imports: [],
  templateUrl: './care-subscription-succes.component.html',
  styleUrl: './care-subscription-succes.component.css',
})
export class CareSubscriptionSuccesComponent {
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
}
