import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
  signal,
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Meta, Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { filter, finalize, switchMap, tap } from 'rxjs/operators';
import { AnimalAidRequest } from '../../../core/models/animalAidRequest';
import { AnimalAidRequestService } from '../../../core/services/animal-aid-request.service';
import { LoadingSpinnerComponent } from '../../../shared/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-animal-aid-request-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule,
    LoadingSpinnerComponent,
  ],

  templateUrl: './animal-aid-request-detail.component.html',
  styleUrl: './animal-aid-request-detail.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AnimalAidRequestDetailComponent {
  private route = inject(ActivatedRoute);
  public router = inject(Router);
  private cdr = inject(ChangeDetectorRef);
  private animalAidRequestService = inject(AnimalAidRequestService);
  public translate = inject(TranslateService);
  loading = signal<boolean>(true);
  private title = inject(Title);
  private meta = inject(Meta);
  //private authService = inject(AuthService);

  animalAidRequestId = toSignal(
    this.route.paramMap.pipe(
      switchMap(params => [params.get('id')]),
      filter((id): id is string => id !== null && id !== undefined)
    )
  );
  animalAidRequest = signal<AnimalAidRequest | undefined>(undefined);
  error = signal<string | null>(null);
  // public isAuthenticated: Signal<boolean> = this.authService.isLoggedIn;
  // user: Signal<User | null> = signal(this.authService.currentUser());

  constructor() {
    effect(() => {
      const animalAidRequestIdValue = this.animalAidRequestId();
      if (!animalAidRequestIdValue) return;
      this.animalAidRequestService
        .getAnimalAidRequestById(animalAidRequestIdValue)
        .pipe(
          tap(() => {
            this.loading.set(true); // на старті потоку
          }),
          finalize(() => {
            this.loading.set(false); // після завершення чи помилки
          })
        )
        .subscribe(
          animalAidRequest => {
            if (!animalAidRequest) {
              this.router.navigate(['/not-found']);
              return;
            }
            this.animalAidRequest.set(animalAidRequest);
            this.cdr.detectChanges();

            this.addJsonLd(animalAidRequest);
            const translatedName = this.translate.instant(
              'animalAidRequest.title',
              {
                value: animalAidRequest.title,
              }
            );
            const translatedDescription = this.translate.instant(
              'animalAidRequest.description',
              {
                value: animalAidRequest.description,
              }
            );

            this.title.setTitle(`${translatedName} - PetCare`);
            this.meta.updateTag({
              name: 'description',
              content: translatedDescription || '',
            });
            this.meta.updateTag({
              property: 'og:title',
              content: translatedName,
            });
            this.meta.updateTag({
              property: 'og:description',
              content: translatedDescription,
            });
            this.meta.updateTag({ property: 'og:type', content: 'Demand' });
            this.meta.updateTag({
              property: 'og:url',
              content: window.location.href,
            });
            // Якщо є фото:
            if (animalAidRequest.photos?.length) {
              this.meta.updateTag({
                property: 'og:image',
                content: animalAidRequest.photos[0],
              });
            }

            this.meta.updateTag({
              name: 'twitter:card',
              content: 'summary_large_image',
            });
            this.meta.updateTag({
              name: 'twitter:title',
              content: translatedName,
            });
            this.meta.updateTag({
              name: 'twitter:description',
              content: translatedDescription,
            });
            this.meta.updateTag({
              name: 'keywords',
              content: `petcare, ${animalAidRequest.title}, ${animalAidRequest.category}, ${animalAidRequest.status},
						${animalAidRequest.estimatedCost} 'UAH'`,
            });
          },
          error => {
            this.error.set(error);
            this.cdr.detectChanges();
          }
        );
    });
  }
  addJsonLd(animalAidRequest: AnimalAidRequest) {
    // Видаляємо попередні JSON-LD теги, щоб уникнути дублювання
    document
      .querySelectorAll('script[type="application/ld+json"]')
      .forEach(el => el.remove());

    const script = document.createElement('script');
    script.type = 'application/ld+json';

    const authorName = animalAidRequest.user
      ? `${animalAidRequest.user.firstName} ${animalAidRequest.user.lastName}`
      : 'Admin';

    const shortDescription = animalAidRequest.description
      ? animalAidRequest.description.split(' ').slice(0, 25).join(' ') // приблизно 150 символів
      : '';

    const jsonLd: Record<string, unknown> = {
      '@context': 'https://schema.org',
      '@type': 'Demand',
      name: animalAidRequest.title,
      description: shortDescription,
      datePosted: new Date(animalAidRequest.createdAt).toISOString(),
      category: animalAidRequest.category,
      availability: 'https://schema.org/InStock',
      author: {
        '@type': 'Person',
        name: authorName,
      },
    };

    if (animalAidRequest.photos?.length) {
      jsonLd['image'] = animalAidRequest.photos;
    }

    const cost = Number(animalAidRequest.estimatedCost);
    if (!isNaN(cost) && cost > 0) {
      jsonLd['priceSpecification'] = {
        '@type': 'PriceSpecification',
        price: cost,
        priceCurrency: 'UAH',
      };
    }

    script.text = JSON.stringify(jsonLd);
    document.head.appendChild(script);
  }
}
