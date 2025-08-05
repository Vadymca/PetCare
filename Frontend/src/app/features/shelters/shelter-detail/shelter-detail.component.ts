import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  effect,
  inject,
  Signal,
  signal,
} from '@angular/core';

import { CommonModule } from '@angular/common';
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
import { ShelterSubscription } from '../../../core/models/shelterSubscriptions';
import { User } from '../../../core/models/user';
import { AuthService } from '../../../core/services/auth.service';
import { ShelterSubscriptionService } from '../../../core/services/shelter-subscription.service';
import { ShelterService } from '../../../core/services/shelter.service';

@Component({
  selector: 'app-shelter-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule],
  templateUrl: './shelter-detail.component.html',
  styleUrl: './shelter-detail.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ShelterDetailComponent {
  private route = inject(ActivatedRoute);
  public router = inject(Router);
  private title = inject(Title);
  private meta = inject(Meta);
  private translate = inject(TranslateService);
  private shelterService = inject(ShelterService);
  private sanitizer = inject(DomSanitizer);
  private cdr = inject(ChangeDetectorRef);
  private authService = inject(AuthService);
  private shelterSubscriptionService = inject(ShelterSubscriptionService);
  private shelterSubscriptionId = '';

  mapUrl = signal<SafeResourceUrl | null>(null); // оголошено з дефолтним значенням

  slug = toSignal(
    this.route.paramMap.pipe(
      switchMap(params => [params.get('slug')]),
      filter((slug): slug is string => slug !== null && slug !== undefined)
    )
  );

  shelter = signal<Shelter | undefined>(undefined);
  public isAuthenticated: Signal<boolean> = this.authService.isLoggedIn;
  user: Signal<User | null> = signal(this.authService.currentUser());
  isSubscribed = false; // Поточна підписка на тварину, по замовчуванню false;
  isSubscriptionChecked = false;
  constructor() {
    effect(() => {
      const slugValue = this.slug();
      if (!slugValue) return;

      this.shelterService.getShelterBySlug(slugValue).subscribe(shelter => {
        if (!shelter) {
          this.router.navigate(['/not-found']);
          return;
        }

        this.shelter.set(shelter);
        const translatedName = this.translate.instant('shelter.name', {
          value: shelter.name,
        });
        const translatedDescription = this.translate.instant(
          'shelter.description',
          {
            value: shelter.address,
          }
        );

        this.setMetaTags(translatedName, translatedDescription);
        this.addJsonLd({
          name: shelter.name,
          description: shelter.address,
          telephone: shelter.contactPhone,
          address: shelter.address,
          url: window.location.href,
        });
        this.isSubscriptionChecked = false;
        console.log('this.user', this.user());
        console.log('this.shelter', this.shelter());
        this.isSubscribedToShelter().subscribe(isSubscribed => {
          this.isSubscribed = isSubscribed;
          this.isSubscriptionChecked = true;
          this.cdr.detectChanges();
        });
        if (shelter.coordinates.lat && shelter.coordinates.lng) {
          const url = `https://maps.google.com/maps?q=${shelter.coordinates.lat},${shelter.coordinates.lng}&z=14&output=embed`;
          this.mapUrl.set(this.sanitizer.bypassSecurityTrustResourceUrl(url));
        } else {
          this.mapUrl.set(null);
        }
      });
    });
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
    this.meta.updateTag({ property: 'og:url', content: window.location.href });

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
    const script = document.createElement('script');
    script.type = 'application/ld+json';

    const jsonLd = {
      '@context': 'https://schema.org',
      '@type': 'LocalBusiness',
      name: data.name,
      description: data.description,
      telephone: data.telephone,
      address: data.address,
      url: data.url,
    };

    script.text = JSON.stringify(jsonLd);
    document.head.appendChild(script);
  }
  isSubscribedToShelter(): Observable<boolean> {
    const shelterValue = this.shelter();
    const userValue = this.user();
    if (!userValue) return of(false);
    if (!shelterValue) return of(false);

    return this.shelterSubscriptionService
      .getShelterSubscriptionsByUserId(userValue.id)
      .pipe(
        map(subscriptions => {
          const found = subscriptions.find(
            s => s.shelterId === shelterValue.id
          );
          this.shelterSubscriptionId = found?.id ?? '';
          this.isSubscribed = !!found;
          return !!found;
        }),
        catchError(err => {
          console.error('Error fetching shelter subscriptions:', err);
          return of(false);
        })
      );
  }
  unsubscribe() {
    if (!this.isSubscribed) return;
    if (this.shelterSubscriptionId === '') return;
    this.shelterSubscriptionService
      .deleteShelterSubscription(this.shelterSubscriptionId)
      .subscribe({
        next: () => {
          this.isSubscribed = false;
          this.shelterSubscriptionId = '';
          this.cdr.detectChanges();
        },
        error: err => {
          console.error('Error deleting shelter subscription:', err);
        },
      });
  }
  subscribe() {
    if (this.isSubscribed) return;
    const shelterValue = this.shelter();
    const userValue = this.user();
    if (!shelterValue || !userValue) return;
    const shelterSubscription: Partial<ShelterSubscription> = {
      shelterId: shelterValue.id,
      userId: userValue.id,
    };

    this.shelterSubscriptionService
      .createShelterSubscription(shelterSubscription)
      .subscribe({
        next: subscription => {
          this.isSubscribed = true;
          this.shelterSubscriptionId = subscription.id;
          this.cdr.detectChanges();
        },
        error: err => {
          console.error('Error craeting shelter subscription:', err);
        },
      });
  }
}
