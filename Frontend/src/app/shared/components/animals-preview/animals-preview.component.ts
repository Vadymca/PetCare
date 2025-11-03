import { CommonModule } from '@angular/common';
import { Component, effect, inject, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { catchError, of } from 'rxjs';
import { Animal } from '../../../core/models/animal';
import { AnimalSubscriptionService } from '../../../core/services/animal-subscription.service';
import { AnimalService } from '../../../core/services/animal.service';
import { AuthService } from '../../../core/services/auth.service';
import { ModalService } from '../../../core/services/modal.service';
import { AnimalCardComponent } from '../animal-card/animal-card.component';
import { SecondaryLargeButtonComponent } from '../buttons/blue/secondary-large-button.component';

@Component({
  selector: 'app-animals-preview',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    SecondaryLargeButtonComponent,
    ReactiveFormsModule,
    AnimalCardComponent,
  ],
  templateUrl: './animals-preview.component.html',
  styleUrl: './animals-preview.component.css',
})
export class AnimalsPreviewComponent {
  router = inject(Router);
  private animalSubscriptionService = inject(AnimalSubscriptionService);
  private animalService = inject(AnimalService);
  private authService = inject(AuthService);
  private authModalService = inject(ModalService);
  private user = this.authService._currentUser;

  animals = signal<Animal[]>([]);

  constructor() {
    // Завантажуємо тварин
    this.animalService.getAnimals().subscribe(result => {
      const animals = result.animals.slice(0, 8).map(animal => ({
        ...animal,
        isChecked: true,
        isFavorite: false,
      }));
      this.animals.set(animals);

      // Завантажуємо фаворити і оновлюємо стани
      this.updateFavorites();
    });

    // Ефект оновлення фаворитів
    effect(() => {
      const currentUser = this.user();
      if (!currentUser) return;

      this.updateFavorites();
    });
  }

  private updateFavorites() {
    if (!this.user()) return;

    this.animalSubscriptionService
      .getFavoriteAnimals()
      .pipe(
        catchError(err => {
          console.error('Error fetching favorite animals:', err);
          return of([]);
        })
      )
      .subscribe(favorites => {
        const favoriteIds = new Set(favorites.map(a => a.id));
        this.animals.update(all =>
          all.map(animal => ({
            ...animal,
            isFavorite: favoriteIds.has(animal.id),
            isChecked: true,
          }))
        );
      });
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

  subscribeToAnimal(animal: Animal) {
    if (animal.isFavorite) return;
    animal.isChecked = false;

    this.animalSubscriptionService
      .createAnimalSubscription(animal.id)
      .subscribe({
        next: subscription => {
          this.animals.update(all =>
            all.map(a =>
              a.id === animal.id
                ? {
                    ...a,
                    isFavorite: true,
                    animalSubscriptionId: subscription.id,
                    isChecked: true,
                  }
                : a
            )
          );
        },
        error: err => {
          console.error('Error creating animal subscription:', err);
          animal.isChecked = true;
        },
      });
  }

  unsubscribeFromAnimal(animal: Animal) {
    if (!animal.isFavorite || !animal.animalSubscriptionId) return;
    animal.isChecked = false;

    this.animalSubscriptionService
      .deleteAnimalSubscription(animal.animalSubscriptionId)
      .subscribe({
        next: () => {
          this.animals.update(all =>
            all.map(a =>
              a.id === animal.id
                ? {
                    ...a,
                    isFavorite: false,
                    animalSubscriptionId: '',
                    isChecked: true,
                  }
                : a
            )
          );
        },
        error: err => {
          console.error('Error deleting animal subscription:', err);
          animal.isChecked = true;
        },
      });
  }

  onSeeAllAnimalsClick() {
    this.router.navigate(['/animals']);
  }

  onAnimalDetailClick(animal: Animal) {
    this.router.navigate(['/animals', animal.slug]);
  }

  trackByAnimalId(index: number, animal: Animal): string {
    return animal.id;
  }
}
