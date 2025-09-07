import { CommonModule } from '@angular/common';
import { Component, effect, inject, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { catchError, map, Observable, of } from 'rxjs';
import { Animal } from '../../../core/models/animal';
import { AnimalSubscription } from '../../../core/models/animalSubscription';
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
  private user = this.authService._currentUser;
  private authModalService = inject(ModalService);
  animals: Animal[] = [];
  private userSubscriptions = signal<AnimalSubscription[]>([]);

  constructor() {
    this.animalService.getAnimals().subscribe(animals => {
      this.animals = animals.slice(0, 8).map(animal => ({
        ...animal,
        isChecked: false, // Гарантуємо ініціалізацію
        isFavorite: false, // Додаємо для безпеки
        animalSubscriptionId: '', // Додаємо для безпеки
      }));
      this.updateFavorites();
    });

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

  isSubscribedToAnimal(animalValue: Animal): Observable<string> {
    const userValue = this.user();
    if (!userValue || !animalValue) return of('');

    return this.animalSubscriptionService
      .getAnimalSubscriptionsByUserId(userValue.id)
      .pipe(
        map(subscriptions => {
          const found = subscriptions.find(s => s.animalId === animalValue.id);
          return found?.id ?? '';
        }),
        catchError(err => {
          console.error('Error fetching animal subscriptions:', err);
          return of('');
        })
      );
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
