import { CommonModule, UpperCasePipe } from '@angular/common';
import { Component, effect, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { catchError, of } from 'rxjs';
import { Animal } from '../../../core/models/animal';
import { AnimalSubscription } from '../../../core/models/animalSubscription';
import { AnimalSubscriptionService } from '../../../core/services/animal-subscription.service';
import { AnimalService } from '../../../core/services/animal.service';
import { AuthService } from '../../../core/services/auth.service';
import { ModalService } from '../../../core/services/modal.service';
import { AnimalCardComponent } from '../animal-card/animal-card.component';
import { IconComponent } from '../icon.component';
import { MultiSelectDropdownComponent } from '../multi-select-dropdown/multi-select-dropdown.component';

@Component({
  selector: 'app-animals-for-care',
  standalone: true,
  imports: [
    MultiSelectDropdownComponent,
    CommonModule,
    TranslateModule,
    UpperCasePipe,
    IconComponent,
    AnimalCardComponent,
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
  animalsService = inject(AnimalService);
  animals: Animal[] = [];
  router = inject(Router);
  authService = inject(AuthService);
  private user = this.authService._currentUser;
  authModalService = inject(ModalService);
  animalService = inject(AnimalService);
  animalSubscriptionService = inject(AnimalSubscriptionService);
  private userSubscriptions = signal<AnimalSubscription[]>([]);
  constructor() {
    this.selectedCostOptions.set(this.costOptions);
    this.selectedSexOptions.set(this.sexOptions);
    this.selectedAgeOptions.set(this.ageOptions);
    this.selecteSizeOptions.set(this.sizeOptions);
    this.getAnimals();

    effect(() => {
      const currentUser = this.user();

      if (currentUser) {
        this.animalSubscriptionService
          .getAnimalSubscriptionsByUserId(currentUser.id)
          .pipe(
            catchError(err => {
              console.error('Error fetching user subscriptions:', err);
              return of([]);
            })
          )
          .subscribe(subscriptions => {
            this.userSubscriptions.set(subscriptions);
            this.updateFavorites();
          });
      } else {
        this.userSubscriptions.set([]);
        this.updateFavorites();
      }
    });
  }

  toggleFilters() {
    console.log('Toggle filters called, current state:', this.filtersOpen());
    this.filtersOpen.update(v => !v);
  }
  onSelectionCostChange($event: string[]) {
    this.selectedCostOptions.set($event);
    this.getAnimals();
  }

  onSelectionAgeChange($event: string[]) {
    this.selectedAgeOptions.set($event);
    this.getAnimals();
  }

  onSelectionSexChange($event: string[]) {
    this.selectedSexOptions.set($event);
    this.getAnimals();
  }
  onSelectionSizeChange($event: string[]) {
    this.selecteSizeOptions.set($event);
    this.getAnimals();
  }
  toggleSterilisationOption() {
    this.sterelisationOptions.set(!this.sterelisationOptions());
    this.getAnimals();
  }
  getAnimals() {
    // TODO get animals with filters
    this.animalService.getAnimals().subscribe(animals => {
      this.animals = animals.slice(0, 4).map(animal => ({
        ...animal,
        isChecked: false, // Гарантуємо ініціалізацію
        isFavorite: false, // Додаємо для безпеки
        animalSubscriptionId: '', // Додаємо для безпеки
      }));
      this.updateFavorites();
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
  private updateFavorites() {
    const subscriptions = this.userSubscriptions();
    this.animals.forEach(animal => {
      animal.isChecked = false;
      const found = subscriptions.find(s => s.animalId === animal.id);
      animal.isFavorite = !!found;
      animal.animalSubscriptionId = found?.id ?? '';
      animal.isChecked = true;
    });
  }
  unsubscribeFromAnimal(animal: Animal) {
    if (!animal.isFavorite || !animal.animalSubscriptionId) {
      console.warn('Cannot unsubscribe: invalid state', animal);
      return;
    }
    animal.isChecked = false;

    this.animalSubscriptionService
      .deleteAnimalSubscription(animal.animalSubscriptionId)
      .subscribe({
        next: () => {
          animal.isFavorite = false;
          animal.animalSubscriptionId = '';
          animal.isChecked = true;
          this.userSubscriptions.update(subs =>
            subs.filter(s => s.id !== animal.animalSubscriptionId)
          );
        },
        error: err => {
          console.error('Error deleting animal subscription:', err);
          animal.isChecked = true;
        },
      });
  }
  subscribeToAnimal(animal: Animal) {
    if (animal.isFavorite) return;
    animal.isChecked = false;

    this.animalSubscriptionService
      .createAnimalSubscription({
        animalId: animal.id,
        userId: this.user()!.id,
      })
      .subscribe({
        next: response => {
          animal.isFavorite = true;
          animal.animalSubscriptionId = response.id;
          animal.isChecked = true;
          this.userSubscriptions.update(subs => [...subs, response]);
        },
        error: err => {
          console.error('Error creating animal subscription:', err);
          animal.isChecked = true;
        },
      });
  }
}
