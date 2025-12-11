import { CommonModule, isPlatformBrowser } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  effect,
  inject,
  PLATFORM_ID,
  Renderer2,
  RendererFactory2,
  Signal,
  signal,
} from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import {
  DomSanitizer,
  Meta,
  SafeResourceUrl,
  Title,
} from '@angular/platform-browser';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Observable, of } from 'rxjs';
import { catchError, filter, map, switchMap } from 'rxjs/operators';
import { Shelter } from '../../../core/models/shelter';
import { User } from '../../../core/models/user';
import { AuthService } from '../../../core/services/auth.service';
import { ModalService } from '../../../core/services/modal.service';
import { ShelterSubscriptionService } from '../../../core/services/shelter-subscription.service';
import { ShelterService } from '../../../core/services/shelter.service';
import { RoundFilledWhiteBlueButtonWithIconComponent } from '../../../shared/components/buttons/round-filled-white-blue-button-with-icon.component';
import { RoundWhiteBlueButtonWithIconComponent } from '../../../shared/components/buttons/round-white-blue-button-with-icon.component';
import { IconComponent } from '../../../shared/components/icon.component';
import { PhotoCollectionsComponent } from '../../../shared/components/photo-collections/photo-collections.component';

@Component({
  selector: 'app-shelter-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule,
    IconComponent,
    PhotoCollectionsComponent,
    RoundFilledWhiteBlueButtonWithIconComponent,
    RoundWhiteBlueButtonWithIconComponent,
  ],
  templateUrl: './shelter-detail.component.html',
  styleUrl: './shelter-detail.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ShelterDetailComponent {
  backBottomClick() {
    this.router.navigate(['contacts']);
  }
  private route = inject(ActivatedRoute);
  public router = inject(Router);
  private title = inject(Title);
  private meta = inject(Meta);
  private translate = inject(TranslateService);
  private shelterService = inject(ShelterService);
  private sanitizer = inject(DomSanitizer);

  private authService = inject(AuthService);
  private shelterSubscriptionService = inject(ShelterSubscriptionService);
  private platformId = inject(PLATFORM_ID);
  private renderer: Renderer2 = inject(RendererFactory2).createRenderer(
    null,
    null
  );
  private modalService = inject(ModalService);
  private destroyRef = inject(DestroyRef);

  mapUrl = signal<SafeResourceUrl | null>(null);

  slug = toSignal(
    this.route.paramMap.pipe(
      switchMap(params => [params.get('slug')]),
      filter((slug): slug is string => slug !== null && slug !== undefined)
    )
  );

  shelter = signal<Shelter | undefined>(undefined);

  public isAuthenticated: Signal<boolean> = this.authService.isLoggedIn;
  user: Signal<User | null> = signal(this.authService._currentUser());
  isSubscribed = signal(false); // Початкове значення isSubscribed false;
  isSubscriptionChecked = signal<boolean>(false);

  constructor() {
    effect(() => {
      const slugValue = this.slug();
      if (!slugValue) return;

      const subscription = this.shelterService
        .getShelterBySlug(slugValue)
        .subscribe(shelter => {
          if (!shelter) {
            this.router.navigate(['/not-found']);
            return;
          }

          this.shelter.set(shelter);

          const translatedName = this.translate.instant('shelter.name', {
            value: shelter.name || '',
          });
          const translatedDescription = this.translate.instant(
            'shelter.description',
            {
              value: shelter.address || '',
            }
          );

          this.setMetaTags(translatedName, translatedDescription);
          if (isPlatformBrowser(this.platformId)) {
            this.addJsonLd({
              name: shelter.name || '',
              description: shelter.address || '',
              telephone: shelter.contactPhone || '',
              address: shelter.address || '',
              url: this.router.url,
            });
          }
          this.isSubscriptionChecked.set(false);
          this.isSubscribedToShelter().subscribe(isSubscribed => {
            this.isSubscribed.set(isSubscribed);
            this.isSubscriptionChecked.set(true);
          });
          if (shelter.coordinates?.lat && shelter.coordinates?.lng) {
            const url = `https://maps.google.com/maps?q=${shelter.coordinates.lat},${shelter.coordinates.lng}&z=14&output=embed`;
            this.mapUrl.set(this.sanitizer.bypassSecurityTrustResourceUrl(url));
          } else {
            this.mapUrl.set(null);
          }
        });

      // Clean up subscription on component destruction
      this.destroyRef.onDestroy(() => {
        subscription.unsubscribe();
      });
    });
  }
  onHeartClick() {
    if (!this.user()) {
      this.modalService.openModal('welcome');
      return;
    }
    this.subscribe();
  }

  onFilledHeartClick() {
    this.unsubscribe();
  }
  private setMetaTags(name: string, description: string) {
    this.title.setTitle(`${name} | PetCare`);
    this.meta.updateTag({ name: 'description', content: description || '' });
    this.meta.updateTag({
      name: 'keywords',
      content: `petcare, shelter, ${name}`,
    });

    this.meta.updateTag({ property: 'og:title', content: name });
    this.meta.updateTag({
      property: 'og:description',
      content: description || `Details about ${name}`,
    });
    this.meta.updateTag({ property: 'og:type', content: 'localBusiness' });
    this.meta.updateTag({ property: 'og:url', content: this.router.url });

    this.meta.updateTag({ name: 'twitter:card', content: 'summary' });
    this.meta.updateTag({ name: 'twitter:title', content: name });
    this.meta.updateTag({
      name: 'twitter:description',
      content: description || '',
    });
  }

  private addJsonLd(data: {
    name: string;
    description: string;
    telephone?: string;
    address?: string;
    url?: string;
  }) {
    const script = this.renderer.createElement('script');
    this.renderer.setAttribute(script, 'type', 'application/ld+json');
    const jsonLd = {
      '@context': 'https://schema.org',
      '@type': 'LocalBusiness',
      name: data.name,
      description: data.description,
      telephone: data.telephone,
      address: data.address,
      url: data.url,
    };
    this.renderer.setProperty(script, 'text', JSON.stringify(jsonLd));
    this.renderer.appendChild(document.head, script);
  }

  isSubscribedToShelter(): Observable<boolean> {
    if (!this.isAuthenticated()) return of(false);
    const shelterValue = this.shelter();
    // const userValue = this.user();
    // if (!userValue || !shelterValue) return of(false);

    return this.shelterSubscriptionService.getMyFavouriteShelters().pipe(
      map(shelters => {
        const found = shelters.find(s => s.id === shelterValue!.id);
        this.isSubscribed.set(!!found);
        return !!found;
      }),
      catchError(err => {
        console.error('Error fetching shelter subscriptions:', err);
        return of(false);
      })
    );
  }

  unsubscribe() {
    if (!this.isSubscribed()) return;
    const shelterValue = this.shelter();
    if (!shelterValue) return;
    this.shelterSubscriptionService
      .deleteShelterSubscription(shelterValue.id)
      .subscribe({
        next: () => {
          this.isSubscribed.set(false);
        },
        error: err => {
          console.error('Error deleting shelter subscription:', err);
        },
      });
  }

  subscribe() {
    if (this.isSubscribed()) return;
    const shelterValue = this.shelter();
    if (!shelterValue) return;

    this.shelterSubscriptionService
      .createShelterSubscription(shelterValue.id)
      .subscribe({
        next: () => {
          this.isSubscribed.set(true);
        },
        error: err => {
          console.error('Error creating shelter subscription:', err);
        },
      });
  }

  onOccupancyClick() {
    this.router.navigate(['shelters', this.shelter()?.slug, 'animals']);
  }
}
