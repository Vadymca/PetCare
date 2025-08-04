import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ShelterSubscription } from '../../core/models/shelterSubscriptions';

@Component({
  selector: 'app-shelter-subscription-card',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule],
  template: `
    @if (shelterSubscription.shelter) {
      <div
        tabindex="0"
        (keydown)="onKeydown($event)"
        (click)="goToShelter()"
        class="block p-4 bg-white rounded-2xl shadow hover:shadow-lg transition mb-5"
      >
        <h2 class="text-xl font-semibold mb-1">
          {{ shelterSubscription.shelter!.name }}
        </h2>
        <p class="text-gray-600 mb-1">
          {{ shelterSubscription.shelter!.address }}
        </p>
        <p class="text-sm text-gray-500">
          {{ shelterSubscription.shelter!.contactPhone }}
        </p>
        <p class="text-gray-600">
          {{ 'SUBSCRIPTION_DATE' | translate }}:
          {{ shelterSubscription.subscribedAt | date: 'mediumDate' }}
        </p>

        <button
          type="button"
          class="mt-2 text-sm text-gray-700 hover:text-orange-300 underline"
          (click)="confirmUnsubscribe($event)"
        >
          {{ 'UNSUBSCRIBE' | translate }}
        </button>
      </div>
    }
  `,
})
export class ShelterSubscriptionCardComponent {
  router = inject(Router);
  translate = inject(TranslateService);
  @Input() shelterSubscription!: ShelterSubscription;
  @Output() deleteSubscription = new EventEmitter<void>();

  confirmUnsubscribe(event: MouseEvent): void {
    event.stopPropagation(); // зупиняємо спливання події, щоб не спрацював клік на <a>
    const message = this.translate.instant('CONFIRM_UNSUBSCRIBE');
    if (confirm(message)) {
      this.deleteSubscription.emit();
    }
  }
  onKeydown(event: KeyboardEvent): void {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault(); // щоб уникнути прокрутки при Space
      this.goToShelter();
    }
  }

  goToShelter(): void {
    this.router.navigate(['/shelters', this.shelterSubscription.shelter!.slug]);
  }
}
