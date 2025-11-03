import { CommonModule, LowerCasePipe } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  Output,
  computed,
} from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { Animal } from '../../../core/models/animal';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { RoundFilledWhiteBlueButtonWithIconComponent } from '../../../shared/components/buttons/round-filled-white-blue-button-with-icon.component';
import { RoundWhiteBlueButtonWithIconComponent } from '../../../shared/components/buttons/round-white-blue-button-with-icon.component';

@Component({
  selector: 'app-animal-card',
  standalone: true,
  imports: [
    PrimaryLargeButtonComponent,
    CommonModule,
    TranslateModule,
    LowerCasePipe,
    RoundWhiteBlueButtonWithIconComponent,
    RoundFilledWhiteBlueButtonWithIconComponent,
  ],
  templateUrl: './animal-card.component.html',
  styleUrl: './animal-card.component.css',
})
export class AnimalCardComponent {
  @Input({ required: true }) animal!: Animal;
  @Output() animalDetailClick = new EventEmitter();
  @Output() heartClick = new EventEmitter();
  readonly isChecked = computed(() => {
    const checked = this.animal.isChecked;

    return checked;
  });

  onAnimalDetailClick() {
    this.animalDetailClick.emit();
  }

  onHeartClick() {
    this.heartClick.emit(this.animal);
  }

  onFilledHeartClick() {
    this.heartClick.emit(this.animal);
  }
}
