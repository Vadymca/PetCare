// features/shelters/shelter-list.component.ts
import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
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
  shelters = toSignal(this.shelterService.getShelters(), { initialValue: [] });
}
