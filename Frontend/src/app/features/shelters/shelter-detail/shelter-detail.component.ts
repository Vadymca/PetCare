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
        this.title.setTitle(shelter.name);
        this.meta.updateTag({
          name: 'description',
          content: shelter.address,
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
