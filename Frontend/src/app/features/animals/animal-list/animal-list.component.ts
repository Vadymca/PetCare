import { CommonModule, UpperCasePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  signal,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { catchError, of } from 'rxjs';
import { Animal } from '../../../core/models/animal';
import { AnimalSubscriptionService } from '../../../core/services/animal-subscription.service';
import { AnimalService } from '../../../core/services/animal.service';
import { AuthService } from '../../../core/services/auth.service';
import { ModalService } from '../../../core/services/modal.service';
import { IconComponent } from '../../../shared/components/icon.component';
import { MultiSelectDropdownComponent } from '../../../shared/components/multi-select-dropdown/multi-select-dropdown.component';
import { PaginationComponent } from '../../../shared/components/pagination/pagination.component';
import { AnimalCardComponent } from '../animal-card/animal-card.component';

interface AnimalFilters {
  page?: number;
  pageSize?: number;
  genders?: string;
  sizes?: string[];
  statuses?: string[];
  isSterilized?: boolean;
  isUndercare?: boolean;
  minAge?: number;
  maxAge?: number;
  careCosts?: string[];
  animalTypeFilter?: string;
}

// Типи
type SexOption = 'BOY' | 'GIRL';
type SizeOption = 'SMALL' | 'MEDIUM' | 'MEDIUM_PLUS' | 'BIG';
type AgeOption = 'UP_TO_1' | '1_TO_5' | '5_OR_MORE';
type CostOption = '600' | '700' | '1000' | '1300' | '2000';
type SpeciesOption = 'ALL_SPECIES' | 'CAT' | 'DOG' | 'OTHER_SPECIES';

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
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private authService = inject(AuthService);
  private authModalService = inject(ModalService);
  private animalSubscriptionService = inject(AnimalSubscriptionService);

  // === Сигнали ===
  currentPage = signal(1);
  selectedSexOptions = signal<SexOption[]>([]);
  selectedSizeOptions = signal<SizeOption[]>([]);
  selectedAgeOptions = signal<AgeOption[]>([]);
  selectedCostOptions = signal<CostOption[]>([]);
  selectedSpeciesOptions = signal<SpeciesOption>('ALL_SPECIES');
  sterelisationOptions = signal(true);
  availableForCareOptions = signal(true);

  // === Дані ===
  animals = signal<Animal[]>([]);
  totalCount = signal(0);
  favoriteAnimals = signal<Animal[]>([]);
  filtersOpen = signal(true);

  // === Константи ===
  readonly pageSize = 16;
  readonly sexOptions = ['BOY', 'GIRL'] as const;
  readonly sizeOptions = ['SMALL', 'MEDIUM', 'MEDIUM_PLUS', 'BIG'] as const;
  readonly ageOptions = ['UP_TO_1', '1_TO_5', '5_OR_MORE'] as const;
  readonly costOptions = ['600', '700', '1000', '1300', '2000'] as const;
  readonly animalSpecies = [
    'CAT',
    'DOG',
    'OTHER_SPECIES',
    'ALL_SPECIES',
  ] as const;
  sizeOptionsSignal = computed(() => [...this.sizeOptions]);
  ageOptionsSignal = computed(() => [...this.ageOptions]);
  costOptionsSignal = computed(() => [...this.costOptions]);
  sexOptionsSignal = computed(() => [...this.sexOptions]);
  totalPages = computed(
    () => Math.ceil(this.totalCount() / this.pageSize) || 1
  );

  // === Мапінг ===
  private readonly FILTER_PARAM_MAP = {
    gender: {
      toUrl: (v: SexOption) => (v === 'BOY' ? 'male' : 'female'),
      fromUrl: (v: string): SexOption => (v === 'male' ? 'BOY' : 'GIRL'),
    },
    size: {
      toUrl: (v: SizeOption) => {
        const map: Record<SizeOption, string> = {
          SMALL: 'small',
          MEDIUM: 'medium',
          MEDIUM_PLUS: 'medium-plus',
          BIG: 'large',
        };
        return map[v];
      },
      fromUrl: (v: string): SizeOption => {
        const map: Record<string, SizeOption> = {
          small: 'SMALL',
          medium: 'MEDIUM',
          'medium-plus': 'MEDIUM_PLUS',
          large: 'BIG',
        };
        return map[v] ?? 'SMALL';
      },
    },
    age: {
      toUrl: (v: AgeOption) => v.toLowerCase().replace(/_/g, '-'),
      fromUrl: (v: string): AgeOption => {
        const normalized = v.toUpperCase().replace(/-/g, '_') as AgeOption;
        return this.ageOptions.includes(normalized) ? normalized : 'UP_TO_1';
      },
    },
    cost: {
      toUrl: (v: CostOption) => v,
      fromUrl: (v: string): CostOption =>
        this.costOptions.includes(v as CostOption) ? (v as CostOption) : '600',
    },
    species: {
      toUrl: (v: SpeciesOption) => {
        const map: Record<SpeciesOption, string> = {
          CAT: 'cats',
          DOG: 'dogs',
          OTHER_SPECIES: 'others',
          ALL_SPECIES: 'all',
        };
        return map[v];
      },
      fromUrl: (v: string): SpeciesOption => {
        const map: Record<string, SpeciesOption> = {
          cats: 'CAT',
          dogs: 'DOG',
          others: 'OTHER_SPECIES',
          all: 'ALL_SPECIES',
        };
        return map[v] ?? 'ALL_SPECIES';
      },
    },
  } as const;

  constructor() {
    // 1. Завантажити з URL
    this.loadFiltersFromUrl();

    // 2. Ефект: реагує на ВСІ зміни фільтрів
    effect(() => {
      // Викликаємо всі сигнали — щоб Angular відстежував
      this.currentPage();
      this.selectedSexOptions();
      this.selectedSizeOptions();
      this.selectedAgeOptions();
      this.selectedCostOptions();
      this.selectedSpeciesOptions();
      this.sterelisationOptions();
      this.availableForCareOptions();
      console.log('EFFECT TRIGGERED');

      this.updateUrl();
      this.getAnimals();
    });

    // 3. Ефект: улюблені
    effect(() => {
      const user = this.authService._currentUser();
      if (user) {
        this.animalSubscriptionService
          .getFavoriteAnimals()
          .pipe(catchError(() => of([])))
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

  private loadFiltersFromUrl() {
    const params = this.route.snapshot.queryParams;

    // Скидання до дефолтів
    this.currentPage.set(1);
    this.selectedSexOptions.set([]);
    this.selectedSizeOptions.set([]);
    this.selectedAgeOptions.set([]);
    this.selectedCostOptions.set([]);
    this.selectedSpeciesOptions.set('ALL_SPECIES');
    this.sterelisationOptions.set(true);
    this.availableForCareOptions.set(true);

    if (params['gender']) {
      const values = ('' + params['gender'])
        .split(',')
        .map(v => this.FILTER_PARAM_MAP.gender.fromUrl(v.trim()))
        .filter((v): v is SexOption => this.sexOptions.includes(v));
      this.selectedSexOptions.set(values);
    }

    if (params['size']) {
      const values = ('' + params['size'])
        .split(',')
        .map(v => this.FILTER_PARAM_MAP.size.fromUrl(v.trim()))
        .filter((v): v is SizeOption => this.sizeOptions.includes(v));
      this.selectedSizeOptions.set(values);
    }

    if (params['age']) {
      const values = ('' + params['age'])
        .split(',')
        .map(v => this.FILTER_PARAM_MAP.age.fromUrl(v.trim()))
        .filter((v): v is AgeOption => this.ageOptions.includes(v));
      this.selectedAgeOptions.set(values);
    }

    if (params['cost']) {
      const values = ('' + params['cost'])
        .split(',')
        .filter((v): v is CostOption =>
          this.costOptions.includes(v as CostOption)
        )
        .map(v => v as CostOption);
      this.selectedCostOptions.set(values);
    }

    if (params['species']) {
      this.selectedSpeciesOptions.set(
        this.FILTER_PARAM_MAP.species.fromUrl(params['species'])
      );
    }

    if (params['sterilized'] === 'true') this.sterelisationOptions.set(true);
    else this.sterelisationOptions.set(false);

    if (params['undercare'] === 'false') this.availableForCareOptions.set(true);
    else this.availableForCareOptions.set(false);

    const page = parseInt(params['page'], 10);
    if (page > 0) this.currentPage.set(page);
  }

  private updateUrl() {
    const params = new URLSearchParams();

    const add = (key: string, value: string) => {
      if (value) params.set(key, value);
    };

    // Gender
    const genders = this.selectedSexOptions();
    if (genders.length > 0 && genders.length < this.sexOptions.length) {
      const values = genders.map(g => this.FILTER_PARAM_MAP.gender.toUrl(g));
      add('gender', values.length === 1 ? values[0] : values.join(','));
    }

    // Size
    const sizes = this.selectedSizeOptions();
    if (sizes.length > 0 && sizes.length < this.sizeOptions.length) {
      add(
        'size',
        sizes.map(s => this.FILTER_PARAM_MAP.size.toUrl(s)).join(',')
      );
    }

    // Age
    const ages = this.selectedAgeOptions();
    if (ages.length > 0 && ages.length < this.ageOptions.length) {
      add('age', ages.map(a => this.FILTER_PARAM_MAP.age.toUrl(a)).join(','));
    }

    // Cost
    const costs = this.selectedCostOptions();
    if (costs.length > 0 && costs.length < this.costOptions.length) {
      add('cost', costs.join(','));
    }

    // Species
    if (this.selectedSpeciesOptions() !== 'ALL_SPECIES') {
      add(
        'species',
        this.FILTER_PARAM_MAP.species.toUrl(this.selectedSpeciesOptions())
      );
    }

    // Sterilized
    if (this.sterelisationOptions()) add('sterilized', 'true');

    // Undercare
    if (this.availableForCareOptions()) add('undercare', 'false');

    // Page
    if (this.currentPage() > 1) add('page', this.currentPage().toString());

    // Оновлення URL
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: Object.fromEntries(params),
      replaceUrl: true,
      queryParamsHandling: '',
    });
  }

  private getAnimals() {
    const filters: Partial<AnimalFilters> = {
      page: this.currentPage(),
      pageSize: this.pageSize,
      statuses: ['Available', 'Reserved', 'InTreatment'],
    };

    const genders = this.selectedSexOptions();
    if (genders.length > 0 && genders.length < this.sexOptions.length) {
      const mapped = genders.map(g => (g === 'BOY' ? 'male' : 'female'));
      filters.genders = mapped.length === 1 ? mapped[0] : mapped.join(',');
    }

    const sizes = this.selectedSizeOptions();
    if (sizes.length > 0 && sizes.length < this.sizeOptions.length) {
      const map: Record<SizeOption, string> = {
        SMALL: 'Small',
        MEDIUM: 'Medium',
        MEDIUM_PLUS: 'MediumPlus',
        BIG: 'Large',
      };
      filters.sizes = sizes.map(s => map[s]);
    }

    const ageRanges = this.selectedAgeOptions();
    if (ageRanges.length > 0 && ageRanges.length < this.ageOptions.length) {
      const mins: number[] = [];
      const maxs: number[] = [];
      ageRanges.forEach(r => {
        if (r === 'UP_TO_1') {
          mins.push(0);
          maxs.push(1);
        } else if (r === '1_TO_5') {
          mins.push(1);
          maxs.push(5);
        } else if (r === '5_OR_MORE') {
          mins.push(5);
          maxs.push(400);
        }
      });
      if (
        !(
          ageRanges.includes('UP_TO_1') &&
          ageRanges.includes('5_OR_MORE') &&
          !ageRanges.includes('1_TO_5')
        )
      ) {
        filters.minAge = Math.min(...mins);
        filters.maxAge = Math.max(...maxs);
      }
    }

    const costs = this.selectedCostOptions();
    if (costs.length > 0 && costs.length < this.costOptions.length) {
      const map: Record<CostOption, string> = {
        '600': 'SixHundred',
        '700': 'SevenHundred',
        '1000': 'OneThousand',
        '1300': 'OneThousandThreeHundred',
        '2000': 'TwoThousand',
      };
      filters.careCosts = costs.map(c => map[c]);
    }

    if (this.sterelisationOptions()) filters.isSterilized = true;
    if (this.availableForCareOptions()) filters.isUndercare = false;

    const species = this.selectedSpeciesOptions();
    if (species !== 'ALL_SPECIES') {
      filters.animalTypeFilter =
        species === 'CAT' ? 'cats' : species === 'DOG' ? 'dogs' : 'others';
    }

    this.animalService.getAnimals(filters).subscribe(result => {
      this.totalCount.set(result.totalCount);
      this.animals.set(
        result.animals.map(a => ({
          ...a,
          isChecked: false,
          isFavorite: false,
        }))
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

  // === UI ===
  setPage(page: number) {
    this.currentPage.set(page);
  }

  onSelectionCostChange($event: string[]) {
    const typed = $event as CostOption[];
    this.selectedCostOptions.set(typed);
  }

  onSelectionAgeChange($event: string[]) {
    const typed = $event as AgeOption[];
    this.selectedAgeOptions.set(typed);
  }

  onSelectionSexChange($event: string[]) {
    const typed = $event as SexOption[];
    this.selectedSexOptions.set(typed);
  }

  onSelectionSizeChange($event: string[]) {
    const typed = $event as SizeOption[];
    this.selectedSizeOptions.set(typed);
  }

  toggleSterilisationOption() {
    this.sterelisationOptions.update(v => !v);
  }

  toggleAvailableForCareOption() {
    this.availableForCareOptions.update(v => !v);
  }

  onCatsFilterClick() {
    this.selectedSpeciesOptions.set('CAT');
  }
  onDogsFilterClick() {
    this.selectedSpeciesOptions.set('DOG');
  }
  onOtherSpeciesFilterClick() {
    this.selectedSpeciesOptions.set('OTHER_SPECIES');
  }
  onAllAnimalsFilterClick() {
    this.selectedSpeciesOptions.set('ALL_SPECIES');
  }

  toggleFilters() {
    this.filtersOpen.update(v => !v);
  }

  onAnimalDetailClick(animal: Animal) {
    this.router.navigate(['/animals', animal.slug]);
  }

  onHeartClick(animal: Animal) {
    if (!this.authService._currentUser()) {
      this.authModalService.openModal('welcome');
      return;
    }

    if (animal.isFavorite) {
      this.unsubscribeFromAnimal(animal);
    } else {
      this.subscribeToAnimal(animal);
    }
  }

  private subscribeToAnimal(animal: Animal) {
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
        error: () => {
          animal.isChecked = true;
        },
      });
  }

  private unsubscribeFromAnimal(animal: Animal) {
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
        error: () => {
          animal.isChecked = true;
        },
      });
  }

  onSeeAllAnimalsClick() {
    this.router.navigate(['/animals'], { queryParams: {} });
  }
}
