import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AnimalAidRequest } from '../../../../core/models/animalAidRequest';
import { AnimalAidRequestService } from '../../../../core/services/animal-aid-request.service';
import { DonateService } from '../../../../core/services/donate.service';
import { ModalService } from '../../../../core/services/modal.service';

@Component({
  selector: 'app-live-donation-collection',
  imports: [],
  templateUrl: './live-donation-collection.component.html',
  styleUrl: './live-donation-collection.component.css',
})
export class LiveDonationCollectionComponent {
  animalAidRequestId = 'ccd793bb-f942-4461-999a-639cd4ffaf25';
  animalAidRequest: AnimalAidRequest | undefined;
  animalAidRequestService = inject(AnimalAidRequestService);
  donationService = inject(DonateService);
  router = inject(Router);
  modalService = inject(ModalService);
  constructor() {
    this.animalAidRequestService
      .getAnimalAidRequestById(this.animalAidRequestId)
      .subscribe(animalAidRequest => {
        this.animalAidRequest = animalAidRequest;
      });
  }
  toDonate() {
    this.donationService.clearDonationData();
    this.donationService.setDonationData({
      animalAidRequestId: this.animalAidRequestId,
    });
    this.router.navigate(['donate']);
    this.modalService.closeModal();
  }
  toAnimalAidRequests() {
    this.router.navigate(['animal-aid-requests']);
    this.modalService.closeModal();
  }
}
