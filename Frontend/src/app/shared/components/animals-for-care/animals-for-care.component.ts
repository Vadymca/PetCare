import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, effect, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
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
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AnimalsForCareComponent {
  private router = inject(Router);
  private authService = inject(AuthService);
  private authModalService = inject(ModalService);
  private animalService = inject(AnimalService);
  private animalSubscriptionService = inject(AnimalSubscriptionService);

  // === Дані ===
  private rawAnimals = signal<Animal[]>([]);
  private favoriteAnimalIds = signal<Set<string>>(new Set());

  // Головний computed — тварини з актуальним isFavorite
  displayedAnimals = computed(() => {
    const favIds = this.favoriteAnimalIds();
    return this.rawAnimals().map(animal => ({
      ...animal,
      isFavorite: favIds.has(animal.id),
      isChecked: true,
    }));
  });

  constructor() {
    this.loadAnimals();

    // Завантажуємо улюблені один раз (якщо юзер є)
    if (this.authService._currentUser()) {
      this.animalSubscriptionService.getFavoriteAnimals().subscribe(favs => {
        this.favoriteAnimalIds.set(new Set(favs.map(a => a.id)));
      });
    }

    // Реакція на логін/вихід — оновлюємо сердечка
    effect(() => {
      const user = this.authService._currentUser();
      if (user) {
        this.animalSubscriptionService.getFavoriteAnimals().subscribe(favs => {
          this.favoriteAnimalIds.set(new Set(favs.map(a => a.id)));
        });
      } else {
        this.favoriteAnimalIds.set(new Set());
      }
    });
  }

  private loadAnimals() {
    this.animalService
      .getAnimals({
        pageSize: 8,
        statuses: ['Available', 'InTreatment'],
        isUndercare: false,
      })
      .subscribe(result => {
        this.rawAnimals.set(result.animals);
      });
  }

  // === Сердечко — миттєве ===
  onHeartClick(animal: Animal) {
    if (!this.authService._currentUser()) {
      this.authModalService.openModal('welcome');
      return;
    }

    const isFavorite = this.favoriteAnimalIds().has(animal.id);

    if (isFavorite) {
      this.animalSubscriptionService.deleteAnimalSubscription(animal.id).subscribe({
        next: () => {
          this.favoriteAnimalIds.update(set => {
            const newSet = new Set(set);
            newSet.delete(animal.id);
            return newSet;
          });
        },
      });
    } else {
      this.animalSubscriptionService.createAnimalSubscription(animal.id).subscribe({
        next: () => {
          this.favoriteAnimalIds.update(set => new Set([...set, animal.id]));
        },
      });
    }
  }

  onAnimalDetailClick(animal: Animal) {
    this.router.navigate(['/animals', animal.slug]);
  }

  onSeeAllAnimalsClick() {
    this.router.navigate(['/animals']);
  }
}