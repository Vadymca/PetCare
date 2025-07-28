// features/shelters/shelter-list.component.ts
import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
} from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { catchError, of } from 'rxjs'; // 🛠️ Додано import
import { ShelterService } from '../../../core/services/shelter.service';

@Component({
  selector: 'app-shelter-list',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule],
  templateUrl: './shelter-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ShelterListComponent {
  private shelterService = inject(ShelterService);
  error = signal<string | null>(null);
  shelters = toSignal(
    this.shelterService.getShelters().pipe(
      catchError(err => {
        this.error.set('FAILED_TO_LOAD_SHELTERS');
        console.error('Error loading shelters:', err);
        return of([]); // Повертаємо порожній список, щоб Signal не впав
      })
    ),
    { initialValue: [] }
  );
}
