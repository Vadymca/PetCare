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
import { Animal } from '../../../core/models/animal';
import { AnimalFiltersDto } from '../../../core/models/animalFiltersDto';
import { AnimalSubscriptionService } from '../../../core/services/animal-subscription.service';
import { AnimalService } from '../../../core/services/animal.service';
import { AuthService } from '../../../core/services/auth.service';
import { ModalService } from '../../../core/services/modal.service';
import { IconComponent } from '../../../shared/components/icon.component';
import { MultiSelectDropdownComponent } from '../../../shared/components/multi-select-dropdown/multi-select-dropdown.component';
import { PaginationComponent } from '../../../shared/components/pagination/pagination.component';
import { AnimalCardComponent } from '../animal-card/animal-card.component';

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

  // === Фільтри ===
  currentPage = signal(1);
  selectedSexOptions = signal<SexOption[]>([]);
  selectedSizeOptions = signal<SizeOption[]>([]);
  selectedAgeOptions = signal<AgeOption[]>([]);
  selectedCostOptions = signal<CostOption[]>([]);
  selectedSpeciesOptions = signal<SpeciesOption>('ALL_SPECIES');
  sterelisationOptions = signal(true);
  availableForCareOptions = signal(true);
  filtersOpen = signal(true);

  // === Дані ===
  private rawAnimals = signal<Animal[]>([]);
  private favoriteAnimalIds = signal<Set<string>>(new Set<string>());
  totalCount = signal(0);

  readonly pageSize = 16;

  readonly sexOptions = ['BOY', 'GIRL'] as const;
  readonly sizeOptions = ['SMALL', 'MEDIUM', 'MEDIUM_PLUS', 'BIG'] as const;
  readonly ageOptions = ['UP_TO_1', '1_TO_5', '5_OR_MORE'] as const;
  readonly costOptions = ['600', '700', '1000', '1300', '2000'] as const;

  sexOptionsSignal = computed(() => [...this.sexOptions]);
  sizeOptionsSignal = computed(() => [...this.sizeOptions]);
  ageOptionsSignal = computed(() => [...this.ageOptions]);
  costOptionsSignal = computed(() => [...this.costOptions]);
  totalPages = computed(
    () => Math.ceil(this.totalCount() / this.pageSize) || 1
  );

  displayedAnimals = computed(() => {
    const favIds = this.favoriteAnimalIds();
    return this.rawAnimals().map(animal => ({
      ...animal,
      isFavorite: favIds.has(animal.id),
      isChecked: true,
    }));
  });

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
    this.loadFiltersFromUrl();

    // Завантажуємо улюблені один раз при вході
    if (this.authService._currentUser()) {
      this.animalSubscriptionService.getFavoriteAnimals().subscribe(favs => {
        this.favoriteAnimalIds.set(new Set(favs.map(a => a.id)));
      });
    }

    // Реакція на зміна будь-якого фільтра
    effect(() => {
      this.currentPage();
      this.selectedSexOptions();
      this.selectedSizeOptions();
      this.selectedAgeOptions();
      this.selectedCostOptions();
      this.selectedSpeciesOptions();
      this.sterelisationOptions();
      this.availableForCareOptions();

      this.updateUrl();
      this.fetchAnimals();
    });
  }

  private loadFiltersFromUrl() {
    const params = this.route.snapshot.queryParams;

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

    if (params['sterilized'] !== undefined) {
      this.sterelisationOptions.set(params['sterilized'] !== 'false');
    }

    if (params['undercare'] !== undefined) {
      this.availableForCareOptions.set(params['undercare'] === 'false');
    }

    const page = parseInt(params['page'] || '1', 10);
    if (page > 0) this.currentPage.set(page);
  }

  private updateUrl() {
    const params = new URLSearchParams();

    const genders = this.selectedSexOptions();
    if (genders.length > 0) {
      params.set(
        'gender',
        genders.map(g => this.FILTER_PARAM_MAP.gender.toUrl(g)).join(',')
      );
    }

    const sizes = this.selectedSizeOptions();
    if (sizes.length > 0) {
      params.set(
        'size',
        sizes.map(s => this.FILTER_PARAM_MAP.size.toUrl(s)).join(',')
      );
    }

    const ages = this.selectedAgeOptions();
    if (ages.length > 0) {
      params.set(
        'age',
        ages.map(a => this.FILTER_PARAM_MAP.age.toUrl(a)).join(',')
      );
    }

    const costs = this.selectedCostOptions();
    if (costs.length > 0) {
      params.set('cost', costs.join(','));
    }

    if (this.selectedSpeciesOptions() !== 'ALL_SPECIES') {
      params.set(
        'species',
        this.FILTER_PARAM_MAP.species.toUrl(this.selectedSpeciesOptions())
      );
    }

    if (!this.sterelisationOptions()) {
      params.set('sterilized', 'false');
    }

    if (!this.availableForCareOptions()) {
      params.set('undercare', 'true');
    }

    if (this.currentPage() > 1) {
      params.set('page', this.currentPage().toString());
    }

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: Object.fromEntries(params),
      replaceUrl: true,
      queryParamsHandling: '',
    });
  }

  private fetchAnimals() {
    const filters: AnimalFiltersDto = {
      page: this.currentPage(),
      pageSize: this.pageSize,
      statuses: ['Available', 'InTreatment', 'Reserved'],
    };

    if (this.selectedSexOptions().length) {
      filters.genders = this.selectedSexOptions().map(g =>
        g === 'BOY' ? 'male' : 'female'
      );
    }

    if (this.selectedSizeOptions().length) {
      const map: Record<SizeOption, string> = {
        SMALL: 'small',
        MEDIUM: 'medium',
        MEDIUM_PLUS: 'mediumPlus',
        BIG: 'large',
      };
      filters.sizes = this.selectedSizeOptions().map(s => map[s]);
    }

    if (this.selectedCostOptions().length) {
      const map: Record<CostOption, string> = {
        '600': 'sixHundred',
        '700': 'sevenHundred',
        '1000': 'oneThousand',
        '1300': 'oneThousandThreeHundred',
        '2000': 'twoThousand',
      };
      filters.careCosts = this.selectedCostOptions().map(c => map[c]);
    }

    if (this.selectedSpeciesOptions() !== 'ALL_SPECIES') {
      filters.animalTypeFilter =
        this.selectedSpeciesOptions() === 'CAT'
          ? 'cats'
          : this.selectedSpeciesOptions() === 'DOG'
            ? 'dogs'
            : 'others';
    }

    if (this.sterelisationOptions()) {
      filters.isSterilized = true;
    }

    if (this.availableForCareOptions()) {
      filters.isUndercare = false;
    }

    if (this.selectedAgeOptions().length) {
      const mins: number[] = [];
      const maxs: number[] = [];
      this.selectedAgeOptions().forEach(r => {
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
      filters.minAge = Math.min(...mins);
      filters.maxAge = Math.max(...maxs);
    }

    this.animalService.getAnimals(filters).subscribe(result => {
      this.totalCount.set(result.totalCount);
      this.rawAnimals.set(result.animals);
    });
  }

  // === Сердечко — миттєве, без помилок типів ===
  onHeartClick(animal: Animal) {
    if (!this.authService._currentUser()) {
      this.authModalService.openModal('welcome');
      return;
    }

    const isFavorite = this.favoriteAnimalIds().has(animal.id);

    if (isFavorite) {
      this.animalSubscriptionService
        .deleteAnimalSubscription(animal.id)
        .subscribe({
          next: () => {
            this.favoriteAnimalIds.update(set => {
              const newSet = new Set(set);
              newSet.delete(animal.id);
              return newSet;
            });
          },
        });
    } else {
      this.animalSubscriptionService
        .createAnimalSubscription(animal.id)
        .subscribe({
          next: () => {
            this.favoriteAnimalIds.update(set => new Set([...set, animal.id]));
          },
        });
    }
  }

  onAnimalDetailClick(animal: Animal) {
    this.router.navigate(['/animals', animal.slug]);
  }

  // === Фільтри ===
  setPage(page: number) {
    this.currentPage.set(page);
  }

  onSelectionCostChange($event: string[]) {
    this.selectedCostOptions.set($event as CostOption[]);
    this.currentPage.set(1);
  }

  onSelectionAgeChange($event: string[]) {
    this.selectedAgeOptions.set($event as AgeOption[]);
    this.currentPage.set(1);
  }

  onSelectionSexChange($event: string[]) {
    this.selectedSexOptions.set($event as SexOption[]);
    this.currentPage.set(1);
  }

  onSelectionSizeChange($event: string[]) {
    this.selectedSizeOptions.set($event as SizeOption[]);
    this.currentPage.set(1);
  }

  toggleSterilisationOption() {
    this.sterelisationOptions.update(v => !v);
    this.currentPage.set(1);
  }

  toggleAvailableForCareOption() {
    this.availableForCareOptions.update(v => !v);
    this.currentPage.set(1);
  }

  onCatsFilterClick() {
    this.selectedSpeciesOptions.set('CAT');
    this.currentPage.set(1);
  }

  onDogsFilterClick() {
    this.selectedSpeciesOptions.set('DOG');
    this.currentPage.set(1);
  }

  onOtherSpeciesFilterClick() {
    this.selectedSpeciesOptions.set('OTHER_SPECIES');
    this.currentPage.set(1);
  }

  onAllAnimalsFilterClick() {
    this.selectedSpeciesOptions.set('ALL_SPECIES');
    this.currentPage.set(1);
  }

  toggleFilters() {
    this.filtersOpen.update(v => !v);
  }
}
