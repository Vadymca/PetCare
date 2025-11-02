import { CommonModule, UpperCasePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  signal,
} from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { catchError, of } from 'rxjs';
import { Animal } from '../../../core/models/animal';
import { AnimalSubscriptionService } from '../../../core/services/animal-subscription.service';
import { AnimalService } from '../../../core/services/animal.service';
import { AuthService } from '../../../core/services/auth.service';
import { ModalService } from '../../../core/services/modal.service';
import { AnimalCardComponent } from '../../../shared/components/animal-card/animal-card.component';
import { IconComponent } from '../../../shared/components/icon.component';
import { MultiSelectDropdownComponent } from '../../../shared/components/multi-select-dropdown/multi-select-dropdown.component';
import { PaginationComponent } from '../../../shared/components/pagination/pagination.component';

@Component({
  selector: 'app-animal-list',
  standalone: true,
  imports: [
    MultiSelectDropdownComponent,
    CommonModule,
    TranslateModule,
    UpperCasePipe,
    IconComponent,
    AnimalCardComponent,
    PaginationComponent,
  ],
  templateUrl: './animal-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AnimalListComponent {
  private animalService = inject(AnimalService);

  error = signal<string | null>(null);
  currentPage = signal(1);

  pageSize = 10;
  totalPages = computed(
    () => Math.ceil(this.totalCount() / this.pageSize) || 1
  );
  setPage($event: number) {
    this.currentPage.set($event);
  }

  costOptions = ['600', '700', '1000', '1300', '2000'];
  sexOptions = ['BOY', 'GIRL'];
  ageOptions = ['UP_TO_1', '1_TO_5', '5_OR_MORE'];
  sizeOptions = ['SMALL', 'MEDIUM', 'MEDIUM_PLUS', 'BIG'];
  animalSpecies = ['CAT', 'DOG', 'OTHER_SPECIES', 'ALL_SPECIES'];

  selectedCostOptions = signal<string[]>([]);
  selectedSexOptions = signal<string[]>([]);
  selectedAgeOptions = signal<string[]>([]);
  selectedSizeOptions = signal<string[]>([]);
  selectedSpeciesOptions = signal<string>('ALL_SPECIES');
  sterelisationOptions = signal(true);
  availableForCareOptions = signal(true);

  filtersOpen = signal(true);

  animals = signal<Animal[]>([]);
  totalCount = signal(0);

  router = inject(Router);
  authService = inject(AuthService);
  private user = this.authService._currentUser;
  authModalService = inject(ModalService);
  animalSubscriptionService = inject(AnimalSubscriptionService);

  favoriteAnimals = signal<Animal[]>([]); // Масив улюблених тварин

  constructor() {
    this.selectedCostOptions.set(this.costOptions);
    this.selectedSexOptions.set(this.sexOptions);
    this.selectedAgeOptions.set(this.ageOptions);
    this.selectedSizeOptions.set(this.sizeOptions);

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

  toggleFilters() {
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
    this.selectedSizeOptions.set($event);
    this.getAnimals();
  }

  toggleSterilisationOption() {
    this.sterelisationOptions.set(!this.sterelisationOptions());
    this.getAnimals();
  }
  toggleAvailableForCareOption() {
    this.availableForCareOptions.set(!this.availableForCareOptions());
    this.getAnimals();
  }

  onOtherSpeciesFilterClick() {
    this.selectedSpeciesOptions.set('OTHER_SPECIES');
  }
  onCatsFilterClick() {
    this.selectedSpeciesOptions.set('CAT');
  }
  onDogsFilterClick() {
    this.selectedSpeciesOptions.set('DOG');
  }
  onAllAnimalsFilterClick() {
    this.selectedSpeciesOptions.set('ALL_SPECIES');
  }

  getAnimals() {
    this.animalService.getAnimals().subscribe(result => {
      this.totalCount.set(result.totalCount);
      this.animals.set(
        result.animals.map(a => ({ ...a, isChecked: false, isFavorite: false }))
      );
      this.updateFavorites();
    });
  }

  private updateFavorites() {
    const favorites = this.favoriteAnimals();
    this.animals.update(animals =>
      animals.map(animal => ({
        ...animal,
        isChecked: true,
        isFavorite: !!favorites.find(f => f.id === animal.id),
      }))
    );
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
