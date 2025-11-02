import { CommonModule } from '@angular/common';
import { Component, effect, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { catchError, of } from 'rxjs';
import { Animal } from '../../../core/models/animal';
import { AnimalSubscriptionService } from '../../../core/services/animal-subscription.service';
import { AnimalService } from '../../../core/services/animal.service';
import { AuthService } from '../../../core/services/auth.service';
import { ModalService } from '../../../core/services/modal.service';
import { AnimalCardComponent } from '../../../shared/components/animal-card/animal-card.component';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';

@Component({
  selector: 'app-animals-for-adoption',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,

    AnimalCardComponent,
    PrimaryLargeButtonComponent,
  ],
  templateUrl: './animals-for-adoption.component.html',
  styleUrl: './animals-for-adoption.component.css',
})
export class AnimalsForAdoptionComponent {
  animals: Animal[] = [];
  favoriteAnimals = signal<Animal[]>([]); // тварини на які підписаний користувач

  router = inject(Router);
  authService = inject(AuthService);
  private user = this.authService._currentUser;
  authModalService = inject(ModalService);
  animalService = inject(AnimalService);
  animalSubscriptionService = inject(AnimalSubscriptionService);

  constructor() {
    this.getAnimals();

    effect(() => {
      const currentUser = this.user();
      if (currentUser) {
        this.animalSubscriptionService
          .getFavoriteAnimals()
          .pipe(
            catchError(err => {
              console.error('Error fetching favorite animals:', err);
              return of([]);
            })
          )
          .subscribe(favorites => {
            this.favoriteAnimals.set(favorites);
            this.updateFavorites();
          });
      } else {
        this.favoriteAnimals.set([]);
        this.updateFavorites();
      }
    });
  }

  getAnimals() {
    this.animalService.getAnimals().subscribe(result => {
      const animals = result.animals;
      this.animals = animals.slice(0, 4).map(animal => ({
        ...animal,
        isChecked: false,
        isFavorite: false,
      }));
      this.updateFavorites();
    });
  }

  private updateFavorites() {
    const favorites = this.favoriteAnimals();
    this.animals.forEach(animal => {
      animal.isFavorite = !!favorites.find(f => f.id === animal.id);
      animal.isChecked = true;
    });
  }

  onAnimalDetailClick(animal: Animal) {
    this.router.navigate(['/animals', animal.slug]);
  }

  onHeartClick(animal: Animal) {
    if (!this.user()) {
      this.authModalService.openModal('welcome');
      return;
    }

    if (animal.isFavorite) {
      this.unsubscribeFromAnimal(animal);
    } else {
      this.subscribeToAnimal(animal);
    }
  }

  unsubscribeFromAnimal(animal: Animal) {
    animal.isChecked = false;
    this.animalSubscriptionService
      .deleteAnimalSubscription(animal.id)
      .subscribe({
        next: () => {
          animal.isFavorite = false;
          animal.isChecked = true;
          this.favoriteAnimals.update(all =>
            all.filter(a => a.id !== animal.id)
          );
        },
        error: err => {
          console.error('Error deleting subscription:', err);
          animal.isChecked = true;
        },
      });
  }

  subscribeToAnimal(animal: Animal) {
    if (animal.isFavorite) return;
    animal.isChecked = false;

    this.animalSubscriptionService
      .createAnimalSubscription(animal.id)
      .subscribe({
        next: () => {
          animal.isFavorite = true;
          animal.isChecked = true;
          this.favoriteAnimals.update(all => [...all, animal]);
        },
        error: err => {
          console.error('Error creating subscription:', err);
          animal.isChecked = true;
        },
      });
  }

  onSeeAllAnimalsClick() {
    this.router.navigate(['/animals']);
  }
}
