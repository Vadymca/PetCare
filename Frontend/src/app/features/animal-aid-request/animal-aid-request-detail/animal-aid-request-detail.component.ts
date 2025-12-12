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
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { filter, finalize, switchMap, tap } from 'rxjs/operators';
import { AnimalAidRequest } from '../../../core/models/animalAidRequest';
import { AnimalAidRequestService } from '../../../core/services/animal-aid-request.service';
import { MetaSsrService } from '../../../core/services/meta-ssr.service'; // Новий сервіс
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
  private metaSsr = inject(MetaSsrService); // Новий сервіс

  loading = signal<boolean>(true);

  animalAidRequestId = toSignal(
    this.route.paramMap.pipe(
      switchMap(params => [params.get('id')]),
      filter((id): id is string => id !== null && id !== undefined)
    )
  );

  animalAidRequest = signal<AnimalAidRequest | undefined>(undefined);
  error = signal<string | null>(null);

  constructor() {
    effect(() => {
      const animalAidRequestIdValue = this.animalAidRequestId();
      if (!animalAidRequestIdValue) return;

      this.animalAidRequestService
        .getAnimalAidRequestById(animalAidRequestIdValue)
        .pipe(
          tap(() => this.loading.set(true)),
          finalize(() => this.loading.set(false))
        )
        .subscribe({
          next: animalAidRequest => {
            if (!animalAidRequest) {
              this.router.navigate(['/not-found']);
              return;
            }

            this.animalAidRequest.set(animalAidRequest);
            this.cdr.detectChanges();

            // НОВІ мета-теги (вже з MetaSsrService)
            this.updateMetaTags(animalAidRequest);

            // Твій JSON-LD
            this.addJsonLd(animalAidRequest);
          },
          error: error => {
            this.error.set(error);
            this.cdr.detectChanges();
          },
        });
    });
  }

  // НОВА ФУНКЦІЯ — заміна всіх старих meta.updateTag + title.setTitle
  private updateMetaTags(request: AnimalAidRequest) {
    const title = `${request.title} — Добродій`;
    const description = request.description
      ? request.description.split(' ').slice(0, 30).join(' ') + '...'
      : `Допоможи притулку зібрати ${request.estimatedCost} грн на ${request.category.toLowerCase()} ❤️`;

    const image =
      request.photos?.[0] ||
      'https://i.pinimg.com/1200x/4f/53/64/4f5364ff9ca98be71bbe2445e53ab17c.jpg';

    const url = `https://dobrodii.onrender.com/animal-aid-requests/${request.id}`;

    this.metaSsr.update(title, description, image, url);
  }

  // Твій JSON-LD залишається без змін — він ідеально працює
  addJsonLd(animalAidRequest: AnimalAidRequest) {
    document
      .querySelectorAll('script[type="application/ld+json"]')
      .forEach(el => el.remove());

    const script = document.createElement('script');
    script.type = 'application/ld+json';

    const shortDescription = animalAidRequest.description
      ? animalAidRequest.description.split(' ').slice(0, 25).join(' ')
      : '';

    const jsonLd: Record<string, unknown> = {
      '@context': 'https://schema.org',
      '@type': 'Demand',
      name: animalAidRequest.title,
      description: shortDescription,
      datePosted: new Date(animalAidRequest.createdAt).toISOString(),
      category: animalAidRequest.category,
      availability: 'https://schema.org/InStock',
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
