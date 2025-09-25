import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, inject, PLATFORM_ID } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { filter } from 'rxjs';
import { ShelterListComponent } from '../../features/shelters/shelter-list/shelter-list.component';
import { PrimaryLargeOrangeButtonComponent } from '../../shared/components/buttons/orange/primary-large-orange-button.component';
import { SocialMediaComponent } from '../../shared/components/social-media/social-media.component';

@Component({
  selector: 'app-contacts',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    ShelterListComponent,
    SocialMediaComponent,
    PrimaryLargeOrangeButtonComponent,
  ],
  templateUrl: './contacts.component.html',
  styleUrl: './contacts.component.css',
})
export class ContactsComponent {
  router = inject(Router);
  goToFeedbackForm() {
    this.router.navigate(['/feedback-form']);
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
