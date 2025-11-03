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
import { AnimalCardComponent } from '../../../features/animals/animal-card/animal-card.component';
import { PrimaryLargeButtonComponent } from '../buttons/blue/primary-large-button.component';

@Component({
  selector: 'app-animals-for-care',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,

    AnimalCardComponent,
    PrimaryLargeButtonComponent,
  ],
  templateUrl: './animals-for-care.component.html',
  styleUrl: './animals-for-care.component.css',
})
export class AnimalsForCareComponent {
  costOptions = ['600', '700', '1000', '1300', '2000'];
  sexOptions = ['BOY', 'GIRL'];
  ageOptions = ['UP_TO_1', '1_TO_5', '5_OR_MORE'];
  sizeOptions = ['SMALL', 'MEDIUM', 'MEDIUM_PLUS', 'BIG'];

  selectedCostOptions = signal<string[]>([]);
  selectedSexOptions = signal<string[]>([]);
  selectedAgeOptions = signal<string[]>([]);
  selecteSizeOptions = signal<string[]>([]);
  sterelisationOptions = signal(true);
  filtersOpen = signal(true);

  animals = signal<Animal[]>([]);

  private animalService = inject(AnimalService);
  private animalSubscriptionService = inject(AnimalSubscriptionService);
  private authService = inject(AuthService);
  private authModalService = inject(ModalService);
  private user = this.authService._currentUser;
  router = inject(Router);

  constructor() {
    this.selectedCostOptions.set(this.costOptions);
    this.selectedSexOptions.set(this.sexOptions);
    this.selectedAgeOptions.set(this.ageOptions);
    this.selecteSizeOptions.set(this.sizeOptions);

    this.getAnimals();

    // ефект оновлення фаворитів при зміні юзера
    effect(() => {
      if (this.user()) {
        this.updateFavorites();
      }
    });
  }

  getAnimals() {
    this.animalService
      .getAnimals({
        pageSize: 8,
        statuses: ['Available', 'InTreatment'],
        isUndercare: false,
      })
      .subscribe(result => {
        const animals = result.animals.map(animal => ({
          ...animal,
          isChecked: true,
          isFavorite: false,
        }));
        this.animals.set(animals);
        this.updateFavorites();
      });
  }

  private updateFavorites() {
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
        next: () => {
          this.animals.update(all =>
            all.map(a =>
              a.id === animal.id
                ? { ...a, isFavorite: true, isChecked: true }
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
    if (!animal.isFavorite) return;
    animal.isChecked = false;

    this.animalSubscriptionService
      .deleteAnimalSubscription(animal.id)
      .subscribe({
        next: () => {
          this.animals.update(all =>
            all.map(a =>
              a.id === animal.id
                ? { ...a, isFavorite: false, isChecked: true }
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
}
