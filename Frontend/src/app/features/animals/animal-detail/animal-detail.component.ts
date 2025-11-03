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
import { Meta, Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Animal } from '../../../core/models/animal';
import { AnimalSubscriptionService } from '../../../core/services/animal-subscription.service';
import { AnimalService } from '../../../core/services/animal.service';
import { AuthService } from '../../../core/services/auth.service';
import { ModalService } from '../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { SecondaryLargeButtonComponent } from '../../../shared/components/buttons/blue/secondary-large-button.component';
import { PrimaryLargeOrangeButtonComponent } from '../../../shared/components/buttons/orange/primary-large-orange-button.component';
import { RoundFilledWhiteBlueButtonWithIconComponent } from '../../../shared/components/buttons/round-filled-white-blue-button-with-icon.component';
import { RoundWhiteBlueButtonWithIconComponent } from '../../../shared/components/buttons/round-white-blue-button-with-icon.component';
import { SmallShareButtonComponent } from '../../../shared/components/buttons/small-share-button/small-share-button.component';
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
  ],
  templateUrl: './animal-detail.component.html',
  styleUrls: ['./animal-detail.component.css'], // зверни увагу на styleUrls (замість styleUrl)
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AnimalDetailComponent implements OnInit {
  animals = signal<Animal[]>([]);

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private animalService = inject(AnimalService);
  private animalSubscriptionService = inject(AnimalSubscriptionService);
  private authModalService = inject(ModalService);
  private authService = inject(AuthService);
  private translate = inject(TranslateService);
  private title = inject(Title);
  private meta = inject(Meta);

  shareInsta = signal<IconName>('shareInsta');
  shareFacebook = signal<IconName>('shareFacebook');
  platformId = inject(PLATFORM_ID);
  isAuthenticated: Signal<boolean> = this.authService.isLoggedIn;
  // slug = toSignal(
  //   this.route.paramMap.pipe(
  //     map(params => params.get('slug')),
  //     filter((slug): slug is string => !!slug)
  //   ),
  //   { initialValue: null }
  // );
  slug = signal<string | null>(null);
  animal = signal<Animal | undefined>(undefined);
  favoriteAnimals = signal<Animal[]>([]);
  isSubscribed = signal(false);
  round(value: number | undefined | null): string {
    return value != null ? value.toFixed(2) : this.translate.instant('UNKNOWN');
  }
  constructor() {
    // Завантаження тварини
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
        this.setMetaTags(animal);
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
        this.setMetaTags(animal);
        this.getAnimals();
      });
      this.getAnimals();
    });
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

  onTakeCare() {
    throw new Error('Method not implemented.');
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
          // фільтруємо, щоб не включати поточну тварину
          .filter(animal => animal.id !== this.animal()!.id)
          // і беремо максимум 4 тварини
          .slice(0, 4)
          // додаємо потрібні поля
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
    this.animalSubscriptionService
      .createAnimalSubscription(animal.id)
      .subscribe({
        next: () => {
          this.loadFavorites(); // оновлюємо список фейворітів
        },
        error: err => console.error('Error subscribing:', err),
      });
  }

  private unsubscribe(animal: Animal) {
    this.animalSubscriptionService
      .deleteAnimalSubscription(animal.id)
      .subscribe({
        next: () => {
          this.loadFavorites(); // оновлюємо список фейворітів
        },
        error: err => console.error('Error unsubscribing:', err),
      });
  }

  private setMetaTags(animal: Animal) {
    const translatedName = this.translate.instant('animal.name', {
      value: animal.name,
    });
    const translatedDescription = this.translate.instant('animal.description', {
      value: animal.description,
    });

    this.title.setTitle(`${translatedName} - PetCare`);
    this.meta.updateTag({
      name: 'description',
      content: translatedDescription || '',
    });
    this.meta.updateTag({ property: 'og:title', content: translatedName });
    this.meta.updateTag({
      property: 'og:description',
      content: translatedDescription,
    });
    this.meta.updateTag({ property: 'og:type', content: 'article' });
    this.meta.updateTag({ property: 'og:url', content: window.location.href });
    if (animal.photos?.length) {
      this.meta.updateTag({ property: 'og:image', content: animal.photos[0] });
    }
    if (!isPlatformBrowser(this.platformId)) return;
    this.meta.updateTag({
      name: 'twitter:card',
      content: 'summary_large_image',
    });
    this.meta.updateTag({ name: 'twitter:title', content: translatedName });
    this.meta.updateTag({
      name: 'twitter:description',
      content: translatedDescription,
    });
    this.meta.updateTag({
      name: 'keywords',
      content: `petcare, ${animal.name}, ${animal.breed?.name}, ${animal.species?.name}`,
    });
  }
}
