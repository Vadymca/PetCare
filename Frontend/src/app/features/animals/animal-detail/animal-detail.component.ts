import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
  Signal,
  signal,
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Meta, Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Observable, of } from 'rxjs';
import { catchError, filter, map, switchMap } from 'rxjs/operators';
import { Animal } from '../../../core/models/animal';
import { AnimalSubscription } from '../../../core/models/animalSubscription';
import { User } from '../../../core/models/user';
import { AnimalSubscriptionService } from '../../../core/services/animal-subscription.service';
import { AnimalService } from '../../../core/services/animal.service';
import { AuthService } from '../../../core/services/auth.service';
import { LoadingSpinnerComponent } from '../../../shared/loading-spinner/loading-spinner.component';
@Component({
  selector: 'app-animal-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule,
    LoadingSpinnerComponent,
  ],
  templateUrl: './animal-detail.component.html',
  styleUrls: ['./animal-detail.component.css'], // зверни увагу на styleUrls (замість styleUrl)
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AnimalDetailComponent {
  private route = inject(ActivatedRoute);
  public router = inject(Router);
  private cdr = inject(ChangeDetectorRef);
  private animalService = inject(AnimalService);
  public translate = inject(TranslateService);
  private title = inject(Title);
  private meta = inject(Meta);
  private authService = inject(AuthService);
  private animalSubscriptionService = inject(AnimalSubscriptionService);
  private animalSubscriptionId = '';
  slug = toSignal(
    this.route.paramMap.pipe(
      switchMap(params => [params.get('slug')]),
      filter((slug): slug is string => slug !== null && slug !== undefined)
    )
  );

  animal = signal<Animal | undefined>(undefined);
  public isAuthenticated: Signal<boolean> = this.authService.isLoggedIn;
  user: Signal<User | null> = signal(this.authService.currentUser());
  isSubscribed = false; // Поточна підписка на тварину, по замовчуванню false;
  isSubscriptionChecked = false;

  constructor() {
    // Ефект завантаження тварини за slug
    effect(() => {
      const slugValue = this.slug();
      if (!slugValue) return;

      this.animalService.getAnimalBySlug(slugValue).subscribe(animal => {
        if (!animal) {
          this.router.navigate(['/not-found']);
          return;
        }

        this.animal.set(animal);
        this.addJsonLd(animal);
        const translatedName = this.translate.instant('animal.name', {
          value: animal.name,
        });
        const translatedDescription = this.translate.instant(
          'animal.description',
          {
            value: animal.description,
          }
        );
        this.isSubscriptionChecked = false;
        console.log('this.user', this.user());
        console.log('this.animal', this.animal());
        this.isSubscribedToAnimal().subscribe(isSubscribed => {
          this.isSubscribed = isSubscribed;
          this.isSubscriptionChecked = true;
          this.cdr.detectChanges();
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
        this.meta.updateTag({
          property: 'og:url',
          content: window.location.href,
        });
        // Якщо є фото:
        if (animal.photos?.length) {
          this.meta.updateTag({
            property: 'og:image',
            content: animal.photos[0],
          });
        }

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
      });
    });
  }
  addJsonLd(animal: Animal) {
    const script = document.createElement('script');
    script.type = 'application/ld+json';
    script.text = JSON.stringify({
      '@context': 'https://schema.org',
      '@type': 'Pet',
      name: animal.name,
      description: animal.description,
      additionalType: animal.species?.name,
      breed: animal.breed?.name,
      image: animal.photos?.[0] || '',
    });
    document.head.appendChild(script);
  }
  isSubscribedToAnimal(): Observable<boolean> {
    const animalValue = this.animal();
    const userValue = this.user();
    if (!userValue) return of(false);
    if (animalValue === undefined) return of(false);

    return this.animalSubscriptionService
      .getAnimalSubscriptionsByUserId(userValue.id)
      .pipe(
        map(subscriptions => {
          const found = subscriptions.find(s => s.animalId === animalValue.id);
          this.animalSubscriptionId = found?.id ?? '';
          this.isSubscribed = !!found;
          return !!found;
        }),
        catchError(err => {
          console.error('Error fetching animal subscriptions:', err);
          return of(false);
        })
      );
  }
  round(value: number | undefined | null): string {
    return value != null ? value.toFixed(2) : this.translate.instant('UNKNOWN');
  }
  unsubscribe() {
    if (!this.isSubscribed) return;
    if (this.animalSubscriptionId === '') return;
    this.animalSubscriptionService
      .deleteAnimalSubscription(this.animalSubscriptionId)
      .subscribe({
        next: () => {
          this.isSubscribed = false;
          this.animalSubscriptionId = '';
          this.cdr.detectChanges();
        },
        error: err => {
          console.error('Error deleting animal subscription:', err);
        },
      });
  }
  subscribe() {
    if (this.isSubscribed) return;
    const animalValue = this.animal();
    const userValue = this.user();
    if (!animalValue || !userValue) return;
    const animalSubscription: Partial<AnimalSubscription> = {
      animalId: animalValue.id,
      userId: userValue.id,
    };

    this.animalSubscriptionService
      .createAnimalSubscription(animalSubscription)
      .subscribe({
        next: subscription => {
          this.isSubscribed = true;
          this.animalSubscriptionId = subscription.id;
          this.cdr.detectChanges();
        },
        error: err => {
          console.error('Error craeting animal subscription:', err);
        },
      });
  }
}
