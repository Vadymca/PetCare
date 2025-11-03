import { CommonModule, UpperCasePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { SecondaryLargeButtonComponent } from '../../../shared/components/buttons/blue/secondary-large-button.component';
import { PrimaryLargeOrangeButtonComponent } from '../../../shared/components/buttons/orange/primary-large-orange-button.component';
import { IconComponent } from '../../../shared/components/icon.component';
import { AnimalsForAdoptionComponent } from '../animals-for-adoption/animals-for-adoption.component';

@Component({
  selector: 'app-adoption',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    IconComponent,
    PrimaryLargeOrangeButtonComponent,
    CommonModule,
    SecondaryLargeButtonComponent,
    AnimalsForAdoptionComponent,
    PrimaryLargeButtonComponent,
  ],
  templateUrl: './adoption.component.html',
  styleUrl: './adoption.component.css',
})
export class AdoptionComponent {
  goToDonation() {
    throw new Error('Method not implemented.');
  }
  goToAdoptionRules() {
    window.open('/adoption-rules', '_blank');
  }
  hovered = signal<number | null>(null); // зберігає id елемента, на якому hover

  onMouseEnter(id: number) {
    this.hovered.set(id);
  }

  onMouseLeave() {
    this.hovered.set(null);
  }
  router = inject(Router);
  menuItems = [
    {
      id: 1,
      title: 'CHOOSE_PET',
      iconName: 'grabPaw',
      hoverIconName: 'grabPawBold',
      class: 'bg-secondary-jordyBlue-3',
    },
    {
      id: 2,
      title: 'WAIT_FOR_CALL',
      iconName: 'grabHand',
      hoverIconName: 'grabHandBold',
      class: 'bg-secondary-jordyBlue-2',
    },
    {
      id: 3,
      title: 'INTRODUCE_YOUR_PET',
      iconName: 'grabPaw',
      hoverIconName: 'grabPawBold',
      class: 'bg-secondary-jordyBlue-3',
    },
    {
      id: 4,
      title: 'PREPARE_SAFE_SPACE',
      iconName: 'grabHand',
      hoverIconName: 'grabHandBold',
      class: 'bg-secondary-jordyBlue-2',
    },
    {
      id: 5,
      title: 'SIGN_AGREEMENT',
      iconName: 'grabPaw',
      hoverIconName: 'grabPawBold',
      class: 'bg-secondary-jordyBlue-3',
    },
  ];
  goToPets() {
    this.router.navigate(['/animals']);
  }
}
