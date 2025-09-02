import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { AnimalAidRequest } from '../../../core/models/animalAidRequest';
import { SecondarySmallOrangeButtonComponent } from "../buttons/orange/secondary-small-orange-button.component";

@Component({
  selector: 'app-home-project-card',
  standalone: true,
  imports: [TranslateModule, CommonModule, SecondarySmallOrangeButtonComponent],
  templateUrl: './home-project-card.component.html',
  styleUrl: './home-project-card.component.css',
})
export class HomeProjectCardComponent {
  @Input({ required: true })
  animalAidRequest!: AnimalAidRequest;
  @Output() projectDetailClick = new EventEmitter();
  isHovered = false;

  onProjectDetailClick() {
    this.projectDetailClick.emit();
  }
  calcDonatedPercent(allreadyDonated: number, estimatedCost: number): number {
    if (!estimatedCost) return 0;
    return Math.round(((allreadyDonated || 0) / estimatedCost) * 100);
  }
}
