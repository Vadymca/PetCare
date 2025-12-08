import { NgClass } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { Animal } from '../../../core/models/animal';
import { AnimalSubscriptionService } from '../../../core/services/animal-subscription.service';
import { AuthService } from '../../../core/services/auth.service';
import { ModalService } from '../../../core/services/modal.service';
import { AnimalCardComponent } from '../../../features/animals/animal-card/animal-card.component';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';

@Component({
  selector: 'app-favorite-animals',
  standalone: true,
  imports: [
    AnimalCardComponent,
    PrimaryLargeButtonComponent,
    NgClass,
    TranslateModule,
  ],
  templateUrl: './favorite-animals.component.html',
})
export class FavoriteAnimalsComponent {
  authService = inject(AuthService);
  authModalService = inject(ModalService);
  private user = this.authService._currentUser;

  animals = signal<Animal[]>([]);

  animalSubscriptionService = inject(AnimalSubscriptionService);
  router = inject(Router);
  constructor() {
    this.getAnimals();
  }
  getAnimals() {
    if (!this.user()) return;
    this.animalSubscriptionService.getFavoriteAnimals().subscribe(favorites => {
      const updatedFavorites = favorites.map(animal => ({
        ...animal,
        isChecked: true,
      }));
      this.animals.set(updatedFavorites);
    });
  }
  onHeartClick(animal: Animal) {
    if (!this.user()) {
      this.authModalService.openModal('welcome');
      return;
    }

    this.unsubscribeFromAnimal(animal);
  }
  onAnimalDetailClick(animal: Animal) {
    this.router.navigate(['/animals', animal.slug]);
  }
  onSeeAllAnimalsClick() {
    this.router.navigate(['/animals']);
  }
  unsubscribeFromAnimal(animal: Animal) {
		if(!this.user()) return;
    this.animalSubscriptionService
      .deleteAnimalSubscription(animal.id)
      .subscribe({
        next: () => {
          this.animals.update(all => all.filter(a => a.id !== animal.id));
        },
        error: err => {
          console.error('Error deleting subscription:', err);
          animal.isChecked = true;
        },
      });
  }
}
