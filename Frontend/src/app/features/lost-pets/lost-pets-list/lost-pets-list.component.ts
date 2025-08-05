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
import { catchError, of } from 'rxjs';
import { LostPetService } from '../../../core/services/lost-pet.service';


@Component({
  selector: 'app-lost-pets-list',
   standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule],
  templateUrl: './lost-pets-list.component.html',
  styleUrl: './lost-pets-list.component.css',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LostPetsListComponent {
private lostPetService = inject(LostPetService);

  error = signal<string | null>(null);

  lostPets = toSignal(
    this.lostPetService.getLostPets().pipe(
      catchError(err => {
        this.error.set('FAILED_TO_LOAD_LOST_PETS');
        console.error('Error loading lostPets:', err);
        return of([]); // Повертаємо порожній список, щоб Signal не впав
      })
    ),
    { initialValue: [] }
  );
}
