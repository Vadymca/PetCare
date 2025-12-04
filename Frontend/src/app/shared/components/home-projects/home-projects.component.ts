import { Component, inject } from '@angular/core';

import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { AnimalAidRequest } from '../../../core/models/animalAidRequest';
import { AnimalAidRequestService } from '../../../core/services/animal-aid-request.service';
import { SecondaryLargeButtonComponent } from '../buttons/blue/secondary-large-button.component';
import { HomeProjectCardComponent } from '../home-project-card/home-project-card.component';

@Component({
  selector: 'app-home-projects',
  standalone: true,
  imports: [
    TranslateModule,
    SecondaryLargeButtonComponent,
    HomeProjectCardComponent,
  ],
  templateUrl: './home-projects.component.html',
  styleUrl: './home-projects.component.css',
})
export class HomeProjectsComponent {
  onProjectDetailClick(animalAidRequest: AnimalAidRequest) {
    this.router.navigate(['/animal-aid-requests', animalAidRequest.id]);
  }
  private router = inject(Router);
  animalAidRequests: AnimalAidRequest[] = [];
  animalAidRequestService = inject(AnimalAidRequestService);
  onSeeAllProjectsClick() {
    this.router.navigate(['/animal-aid-requests']);
  }
  constructor() {
    try {
      this.animalAidRequestService
        .getAnimalAidRequests()
        .subscribe(animalAidRequests => {
          this.animalAidRequests = animalAidRequests.slice(0, 4);
        });
    } catch (e) {
      console.log(e);
    }
  }
}
