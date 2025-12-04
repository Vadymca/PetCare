import { CommonModule } from '@angular/common';
import {
  Component,
  computed,
  EventEmitter,
  inject,
  input,
  Output,
} from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { Guardianship } from '../../../core/models/guardianship';
import { AnimalCardComponent } from '../../../features/animals/animal-card/animal-card.component';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { PrimaryLargeOrangeButtonComponent } from '../../../shared/components/buttons/orange/primary-large-orange-button.component';

@Component({
  selector: 'app-guardianship-card',
  imports: [
    AnimalCardComponent,
    TranslateModule,
    CommonModule,
    PrimaryLargeButtonComponent,
    PrimaryLargeOrangeButtonComponent,
  ],
  templateUrl: './guardianship-card.component.html',
  styleUrl: './guardianship-card.component.css',
})
export class GuardianshipCardComponent {
  toDeleteGuardianship() {
    this.deleteGuardianship.emit();
  }
  toContactUs() {
    this.router.navigate(['/feedback-form']);
  }
  toChangePayment() {
    this.editPaymentData.emit();
  }
  toAdoption() {
    this.adopt.emit();
  }
  onHeartClick() {
    this.toggleFavourite.emit(this.animalSignal().id);
  }
  onAnimalDetailClick() {
    this.router.navigate(['/animals', this.animalSignal().slug]);
  }
  guardianship = input.required<Guardianship>();
  animalSignal = computed(() => this.guardianship().animal);
  @Output() toggleFavourite = new EventEmitter<string>();
  @Output() adopt = new EventEmitter<void>();
  @Output() editPaymentData = new EventEmitter<void>();
  @Output() deleteGuardianship = new EventEmitter<void>();
  @Output() contactUs = new EventEmitter<void>();
  router = inject(Router);
}
