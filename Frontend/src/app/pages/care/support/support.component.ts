import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, inject, PLATFORM_ID, signal } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { AnimalsForCareComponent } from '../../../shared/components/animals-for-care/animals-for-care.component';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { PrimarySmallButtonComponent } from '../../../shared/components/buttons/blue/primary-small-button.component';
import { EventOrganizationComponent } from '../../../shared/components/event-organization/event-organization.component';
import { FinancialSupportComponent } from '../../../shared/components/financial-support/financial-support.component';
import { IconComponent } from '../../../shared/components/icon.component';
import { SupportVolunteeringComponent } from '../../../shared/components/support-volunteering/support-volunteering.component';
import { filter } from 'rxjs';

@Component({
  selector: 'app-support',
  standalone: true,
  imports: [
    IconComponent,
    CommonModule,
    TranslateModule,
    FinancialSupportComponent,
    SupportVolunteeringComponent,
    PrimaryLargeButtonComponent,
    PrimarySmallButtonComponent,
    RouterModule,
    EventOrganizationComponent,

    AnimalsForCareComponent,
  ],
  templateUrl: './support.component.html',
  styleUrl: './support.component.css',
})
export class SupportComponent {
  callEventSpecialist() {
    window.location.href = 'tel:+380509997766';
  }
  callCorrectionSpecialist() {
    window.location.href = 'tel:+380502228822';
  }
  callCoordinator() {
    window.location.href = 'tel:+380503334455';
  }
  router = inject(Router);
  goToAnimals() {
    this.router.navigate(['animals']);
  }
  menuItems = [
    {
      id: 1,
      title: 'BECOME_VOLUNTEER',
      iconName: 'grabHand',
      hoverIconName: 'grabHandBold',
      class: 'bg-secondary-chileanFire-4',
    },
    {
      id: 2,
      title: 'ADOPT_PET',
      iconName: 'grabPaw',
      hoverIconName: 'grabPawBold',
      class: 'bg-primary-orange',
    },
    {
      id: 3,
      title: 'FINANCIAL_SUPPORT',
      iconName: 'grabHand',
      hoverIconName: 'grabHandBold',
      class: 'bg-secondary-chileanFire-3',
    },
    {
      id: 4,
      title: 'TAKE_PET_UNDER_CARE1',
      iconName: 'grabPaw',
      hoverIconName: 'grabPawBold',
      class: 'bg-primary-light-orange',
    },
    {
      id: 5,
      title: 'MAKE_EVENT',
      iconName: 'grabHand',
      hoverIconName: 'grabHandBold',
      class: 'bg-secondary-treePoppy-3',
    },
    {
      id: 6,
      title: 'SPECIALIST',
      iconName: 'grabPaw',
      hoverIconName: 'grabPawBold',
      class: 'bg-secondary-chileanFire-2',
    },
  ];
  hovered = signal<number | null>(null); // зберігає id елемента, на якому hover

  onMouseEnter(id: number) {
    this.hovered.set(id);
  }

  onMouseLeave() {
    this.hovered.set(null);
  }
  scrollToSection(id: number | string) {
    const section = document.getElementById(`section-${id}`);
    if (section) {
      section.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }

  platformId = inject(PLATFORM_ID);
  constructor() {
    if (isPlatformBrowser(this.platformId)) {
      this.router.events
        .pipe(filter(event => event instanceof NavigationEnd))
        .subscribe(() => {
          window.scrollTo({ top: 0, behavior: 'auto' });
        });
    }
  }
}
