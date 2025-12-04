import { NgClass } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { Shelter } from '../../../core/models/shelter';
import { ShelterSubscriptionService } from '../../../core/services/shelter-subscription.service';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { SecondaryLargeButtonComponent } from '../../../shared/components/buttons/blue/secondary-large-button.component';

@Component({
  selector: 'app-favorite-shelters',
  standalone: true,
  imports: [
    TranslateModule,
    SecondaryLargeButtonComponent,
    PrimaryLargeButtonComponent,
    NgClass,
  ],
  templateUrl: './favorite-shelters.component.html',
})
export class FavoriteSheltersComponent {
  router = inject(Router);
  shelters = signal<Shelter[]>([]);
  shelterSubscriptionService = inject(ShelterSubscriptionService);
  visitShelter(shelter: Shelter) {
    this.router.navigate(['/shelters', shelter.slug]);
  }
  constructor() {
    this.loadShelters();
  }
  loadShelters() {
    this.shelterSubscriptionService
      .getMyFavouriteShelters()
      .subscribe(shelters => this.shelters.set(shelters));
  }
  goToShelters() {
    this.router.navigate(['/contacts']);
  }
}
