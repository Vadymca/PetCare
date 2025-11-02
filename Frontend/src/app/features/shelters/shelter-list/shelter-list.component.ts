// features/shelters/shelter-list.component.ts
import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
} from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { catchError, map, of } from 'rxjs'; // 🛠️ Додано import
import { Shelter } from '../../../core/models/shelter';
import { ShelterService } from '../../../core/services/shelter.service';
import { SecondaryLargeButtonComponent } from '../../../shared/components/buttons/blue/secondary-large-button.component';

@Component({
  selector: 'app-shelter-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule,

    SecondaryLargeButtonComponent,
  ],
  templateUrl: './shelter-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ShelterListComponent {
  private router = inject(Router);
  visitShelter(shelter: Shelter) {
    this.router.navigate(['/shelters', shelter.slug]);
  }
  private shelterService = inject(ShelterService);
  error = signal<string | null>(null);
  shelters = toSignal(
    this.shelterService.getShelters().pipe(
      map(response => response.shelters),
      catchError(err => {
        this.error.set('FAILED_TO_LOAD_SHELTERS');
        console.error('Error loading shelters:', err);
        return of([]); // Повертаємо порожній список, щоб Signal не впав
      })
    ),
    { initialValue: [] }
  );
}
