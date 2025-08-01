import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
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
import { filter, switchMap } from 'rxjs/operators';
import { Shelter } from '../../../core/interfaces/shelter';
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

  mapUrl = signal<SafeResourceUrl | null>(null); // оголошено з дефолтним значенням

  slug = toSignal(
    this.route.paramMap.pipe(
      switchMap(params => [params.get('slug')]),
      filter((slug): slug is string => slug !== null && slug !== undefined)
    )
  );

  shelter = signal<Shelter | undefined>(undefined);

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

        if (shelter.coordinates.lat && shelter.coordinates.lng) {
          const url = `https://maps.google.com/maps?q=${shelter.coordinates.lat},${shelter.coordinates.lng}&z=14&output=embed`;
          this.mapUrl.set(this.sanitizer.bypassSecurityTrustResourceUrl(url));
        } else {
          this.mapUrl.set(null);
        }
      });
    });
  }
}
