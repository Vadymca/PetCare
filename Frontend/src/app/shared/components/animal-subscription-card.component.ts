import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AnimalSubscription } from '../../core/models/animalSubscription';

@Component({
  selector: 'app-animal-subscription-card',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule],
  template: `
    @if (animalSubscription.animal) {
      <div
        tabindex="0"
        (keydown)="onKeydown($event)"
        class="block p-4 bg-white rounded-2xl shadow hover:shadow-lg transition cursor-pointer"
        (click)="goToAnimal()"
      >
        <h2 class="text-xl font-semibold">
          {{ animalSubscription.animal!.name }}
        </h2>
        <p class="text-gray-600">
          {{
            animalSubscription.animal!.species?.name || ('UNKNOWN' | translate)
          }}
          —
          {{
            animalSubscription.animal!.breed?.name || ('UNKNOWN' | translate)
          }}
        </p>
        <p class="text-gray-600">
          {{ 'AGE' | translate }}:
          <ng-container
            *ngIf="animalSubscription.animal!.age?.length === 2; else noAge"
          >
            {{ animalSubscription.animal!.age[0] }}
            {{
              animalSubscription.animal!.age[0] === 1
                ? ('YEAR_SINGULAR' | translate)
                : ('YEAR_PLURAL' | translate)
            }},
            {{ animalSubscription.animal!.age[1] }}
            {{
              animalSubscription.animal!.age[1] === 1
                ? ('MONTH_SINGULAR' | translate)
                : ('MONTH_PLURAL' | translate)
            }}
          </ng-container>
          <ng-template #noAge>{{ 'UNKNOWN' | translate }}</ng-template>
        </p>
        <p class="text-gray-600">
          {{ 'SUBSCRIPTION_DATE' | translate }}:
          {{ animalSubscription.subscribedAt | date: 'mediumDate' }}
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
export class AnimalSubscriptionCardComponent {
  router = inject(Router);
  translate = inject(TranslateService);
  @Input() animalSubscription!: AnimalSubscription;
  @Output() deleteSubscription = new EventEmitter<void>();

  goToAnimal(): void {
    if (this.animalSubscription.animal?.slug) {
      this.router.navigate(['/animals', this.animalSubscription.animal.slug]);
    }
  }

  confirmUnsubscribe(event: MouseEvent): void {
    event.stopPropagation(); // Зупиняємо спливання кліку, щоб не викликати goToAnimal
    event.preventDefault(); // Для надійності, щоб браузер нічого не виконував по замовчуванню

    const message = this.translate.instant('CONFIRM_UNSUBSCRIBE');
    if (confirm(message)) {
      this.deleteSubscription.emit();
    }
  }
  onKeydown(event: KeyboardEvent): void {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault(); // щоб уникнути прокрутки при Space
      this.goToAnimal();
    }
  }
}
