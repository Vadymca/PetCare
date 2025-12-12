import { isPlatformBrowser } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
  OnInit,
  PLATFORM_ID,
  Signal,
  signal,
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Animal } from '../../../core/models/animal';
import { PaymentScope } from '../../../core/models/liqPayCheckoutRequest';
import { AnimalSubscriptionService } from '../../../core/services/animal-subscription.service';
import { AnimalService } from '../../../core/services/animal.service';
import { AuthService } from '../../../core/services/auth.service';
import { GuardianshipService } from '../../../core/services/guardianship.service';
import { LiqPayService } from '../../../core/services/liq-pay-service.service';
import { MetaSsrService } from '../../../core/services/meta-ssr.service'; // Новий сервіс
import { ModalService } from '../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { SecondaryLargeButtonComponent } from '../../../shared/components/buttons/blue/secondary-large-button.component';
import { PrimaryLargeOrangeButtonComponent } from '../../../shared/components/buttons/orange/primary-large-orange-button.component';
import { RoundFilledWhiteBlueButtonWithIconComponent } from '../../../shared/components/buttons/round-filled-white-blue-button-with-icon.component';
import { RoundWhiteBlueButtonWithIconComponent } from '../../../shared/components/buttons/round-white-blue-button-with-icon.component';
import { SmallShareButtonComponent } from '../../../shared/components/buttons/small-share-button/small-share-button.component';
import { ConfirmModalComponent } from '../../../shared/components/confirm-modal/confirm-modal.component';
import { IconComponent } from '../../../shared/components/icon.component';
import { PhotoCollectionsComponent } from '../../../shared/components/photo-collections/photo-collections.component';
import { LoadingSpinnerComponent } from '../../../shared/loading-spinner/loading-spinner.component';
import { AnimalCardComponent } from '../animal-card/animal-card.component';

type IconName = 'shareInsta' | 'shareFacebook';

@Component({
  selector: 'app-animal-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule,
    LoadingSpinnerComponent,
    PrimaryLargeButtonComponent,
    SecondaryLargeButtonComponent,
    PhotoCollectionsComponent,
    IconComponent,
    PrimaryLargeOrangeButtonComponent,
    RoundFilledWhiteBlueButtonWithIconComponent,
    RoundWhiteBlueButtonWithIconComponent,
    SmallShareButtonComponent,
    AnimalCardComponent,
    ConfirmModalComponent,
  ],
  templateUrl: './animal-detail.component.html',
  styleUrls: ['./animal-detail.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AnimalDetailComponent implements OnInit {
  animals = signal<Animal[]>([]);

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private guardianshipService = inject(GuardianshipService);
  private liqPayService = inject(LiqPayService);
  private animalService = inject(AnimalService);
  private animalSubscriptionService = inject(AnimalSubscriptionService);
  private authModalService = inject(ModalService);
  private authService = inject(AuthService);
  private translate = inject(TranslateService);
  private metaSsr = inject(MetaSsrService); // Новий сервіс для SSR-мета-тегів
  private platformId = inject(PLATFORM_ID);

  showTakeCareModalWindow = signal(false);
  shareInsta = signal<IconName>('shareInsta');
  shareFacebook = signal<IconName>('shareFacebook');
  isAuthenticated: Signal<boolean> = this.authService.isLoggedIn;

  slug = signal<string | null>(null);
  animal = signal<Animal | undefined>(undefined);
  favoriteAnimals = signal<Animal[]>([]);
  isSubscribed = signal(false);

  round(value: number | undefined | null): string {
    return value != null ? value.toFixed(2) : this.translate.instant('UNKNOWN');
  }

  constructor() {
    effect(() => {
      const slugValue = this.slug();
      if (!slugValue) return;

      this.animalService.getAnimalBySlug(slugValue).subscribe(animal => {
        if (!animal) {
          this.router.navigate(['/not-found']);
          return;
        }
        this.animal.set(animal);
        this.loadFavorites();
        this.updateSubscriptionStatus();
        this.updateMetaTags(animal); // Нова функція
        this.getAnimals();
      });
    });
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const newSlug = params.get('slug');
      if (!newSlug) return;
      this.slug.set(newSlug);

      this.animalService.getAnimalBySlug(newSlug).subscribe(animal => {
        if (!animal) {
          this.router.navigate(['/not-found']);
          return;
        }
        this.animal.set(animal);
        this.loadFavorites();
        this.updateSubscriptionStatus();
        this.updateMetaTags(animal);
        this.getAnimals();
      });
      this.getAnimals();
    });
  }

  private updateMetaTags(animal: Animal) {
    const title = `${animal.name} — Добродій`;

    const breed = animal.breed?.name ? `, ${animal.breed.name}` : '';
    const description = `Допоможіть ${animal.name} знайти дім ❤️ ${breed}`;
    const image =
      animal.photos?.[0] ||
      'https://i.pinimg.com/1200x/b0/55/b5/b055b5ebd3f715cc424caae42b4fa968.jpg';
    const url = `https://dobrodii.onrender.com/animals/${animal.slug}`;

    this.metaSsr.update(title, description, image, url);
  }

  onShareFacebookClick() {
    if (!isPlatformBrowser(this.platformId)) return;

    const currentAnimal = this.animal();
    if (!currentAnimal) return;

    const url = encodeURIComponent(window.location.href);
    const text = encodeURIComponent(
      `Check out ${currentAnimal.name} on PetCare!`
    );
    const shareUrl = `https://www.facebook.com/sharer/sharer.php?u=${url}&quote=${text}`;

    window.open(shareUrl, '_blank', 'width=600,height=400');
  }

  onShareInstaClick() {
    if (!isPlatformBrowser(this.platformId)) return;

    const currentAnimal = this.animal();
    if (!currentAnimal) return;

    const url = encodeURIComponent(window.location.href);
    navigator.clipboard.writeText(url).then(() => {
      alert(this.translate.instant('LINK_COPIED'));
    });
  }

  onTakeCare($event: boolean) {
    if ($event) {
      if (!this.isAuthenticated()) {
        this.authModalService.openModal('welcome');
        return;
      }
      try {
        this.guardianshipService
          .createGuardianship(this.animal()!.id)
          .subscribe(guardianship => {
            if (guardianship.status === 'RequiresPayment') {
              this.liqPayService.startPayment({
                scope: 'guardianship' as PaymentScope,
                isRecurring: true,
                entityId: guardianship.id,
              });

              this.router.navigate(['/payment/details']);
            }
          });
      } catch (err) {
        console.error(err);
      }
    }
    this.showTakeCareModalWindow.set(false);
  }

  onTakeHome() {
    throw new Error('Method not implemented.');
  }

  getAnimals() {
    this.animalService
      .getAnimals({
        pageSize: 5,
        statuses: ['Available'],
        isUndercare: false,
      })
      .subscribe(result => {
        const animals = result.animals
          .filter(animal => animal.id !== this.animal()!.id)
          .slice(0, 4)
          .map(animal => ({
            ...animal,
            isChecked: true,
            isFavorite: false,
          }));

        this.animals.set(animals);
        this.updateFavorites();
      });
  }

  private updateFavorites() {
    if (!this.isAuthenticated()) return;

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

  private loadFavorites() {
    if (this.isAuthenticated()) {
      this.animalSubscriptionService.getFavoriteAnimals().subscribe(animals => {
        this.favoriteAnimals.set(animals);
        this.updateSubscriptionStatus();
      });
    }
  }

  onOtherHeartClick(animal: Animal) {
    if (!this.authService.isLoggedIn()) {
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
    if (!this.isAuthenticated()) return;
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
    if (!this.isAuthenticated()) return;
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

  private updateSubscriptionStatus() {
    if (!this.isAuthenticated()) return;
    const currentAnimal = this.animal();
    if (!currentAnimal) return;
    const isFav = this.favoriteAnimals().some(a => a.id === currentAnimal.id);
    this.isSubscribed.set(isFav);
  }

  onHeartClick() {
    if (!this.authService.isLoggedIn()) {
      this.authModalService.openModal('welcome');
      return;
    }

    const currentAnimal = this.animal();
    if (!currentAnimal) return;

    if (this.isSubscribed()) {
      this.unsubscribe(currentAnimal);
    } else {
      this.subscribe(currentAnimal);
    }
  }

  private subscribe(animal: Animal) {
    if (!this.isAuthenticated()) return;
    this.animalSubscriptionService
      .createAnimalSubscription(animal.id)
      .subscribe({
        next: () => {
          this.loadFavorites();
        },
        error: err => console.error('Error subscribing:', err),
      });
  }

  private unsubscribe(animal: Animal) {
    if (!this.isAuthenticated()) return;
    this.animalSubscriptionService
      .deleteAnimalSubscription(animal.id)
      .subscribe({
        next: () => {
          this.loadFavorites();
        },
        error: err => console.error('Error unsubscribing:', err),
      });
  }

  showModal() {
    this.showTakeCareModalWindow.set(true);
  }
}
