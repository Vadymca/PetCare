import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
  signal,
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { toSignal } from '@angular/core/rxjs-interop';
import { Meta, Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { filter, switchMap } from 'rxjs/operators';
import { AnimalDetail } from '../../../core/interfaces/animal-detail';
import { AnimalService } from '../../../core/services/animal.service';
@Component({
  selector: 'app-animal-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule],
  templateUrl: './animal-detail.component.html',
  styleUrls: ['./animal-detail.component.css'], // зверни увагу на styleUrls (замість styleUrl)
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AnimalDetailComponent {
  private route = inject(ActivatedRoute);
  public router = inject(Router);
  private animalService = inject(AnimalService);
  public translate = inject(TranslateService);
  private title = inject(Title);
  private meta = inject(Meta);

  slug = toSignal(
    this.route.paramMap.pipe(
      switchMap(params => [params.get('slug')]),
      filter((slug): slug is string => slug !== null && slug !== undefined)
    )
  );

  animal = signal<AnimalDetail | undefined>(undefined);

  constructor() {
    effect(() => {
      const slugValue = this.slug();
      if (!slugValue) return;

      this.animalService
        .getAnimalDetailBySlug(slugValue)
        .subscribe(animalDetail => {
          if (!animalDetail) {
            // Редірект, якщо тварина не знайдена
            this.router.navigate(['/animals']);
            return;
          }

          this.animal.set(animalDetail);

          // Встановлюємо SEO мета-теги, використовуючи реальні дані тварини
          const translatedName = this.translate.instant('animal.name', {
            value: animalDetail.name,
          });

          const translatedDescription = this.translate.instant(
            'animal.description',
            {
              value: animalDetail.description,
            }
          );

          this.title.setTitle(`${translatedName} - PetCare`);
          this.meta.updateTag({
            name: 'description',
            content: translatedDescription || '',
          });
        });
    });
  }
  round(value: number | undefined | null): string {
    return value != null ? value.toFixed(2) : this.translate.instant('UNKNOWN');
  }
}
