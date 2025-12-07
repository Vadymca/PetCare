import { CommonModule, UpperCasePipe } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { Guardianship } from '../../core/models/guardianship';
import { PaymentScope } from '../../core/models/liqPayCheckoutRequest';
import { AnimalSubscriptionService } from '../../core/services/animal-subscription.service';
import { GuardianshipService } from '../../core/services/guardianship.service';
import { LiqPayService } from '../../core/services/liq-pay-service.service';
import { PrimaryLargeOrangeButtonComponent } from '../../shared/components/buttons/orange/primary-large-orange-button.component';
import { ConfirmModalComponent } from '../../shared/components/confirm-modal/confirm-modal.component';
import { IconComponent } from '../../shared/components/icon.component';
import { GuardianshipCardComponent } from './guardianship-card/guardianship-card.component';
@Component({
  selector: 'app-guardianships',
  standalone: true,
  imports: [
    TranslateModule,
    IconComponent,
    CommonModule,
    UpperCasePipe,
    PrimaryLargeOrangeButtonComponent,
    GuardianshipCardComponent,
    ConfirmModalComponent,
  ],
  templateUrl: './guardianships.component.html',
  styleUrl: './guardianships.component.css',
})
export class GuardianshipsComponent {
  router = inject(Router);
  showModal = signal(false);
  guardianshipService = inject(GuardianshipService);
  private rawGuardianships = signal<Guardianship[]>([]);
  private favoriteAnimalIds = signal<Set<string>>(new Set());
  guardianships = computed(() => {
    const favIds = this.favoriteAnimalIds();

    return this.rawGuardianships().map(g => ({
      ...g,
      animal: {
        ...g.animal,
        isFavorite: favIds.has(g.animal.id),
        isChecked: true,
        photo: g.animal.photos[0] || 'assets/default-animal.jpg',
        age: this.calculateAgeParts(g.animal.birthday),
      },
    }));
  });
  cancelationGuardianshipId = signal('');
  animalSubscriptionService = inject(AnimalSubscriptionService);
  liqpayService = inject(LiqPayService);
  constructor() {
    this.loadGuardianships();
    this.loadFavoriteAnimalIds();
    // ← ТИМЧАСОВО ДЛЯ ДІАГНОСТИКИ
    // effect(() => {
    //   console.log('favoriteAnimalIds changed:', [...this.favoriteAnimalIds()]);
    //   console.log('guardianships() length:', this.guardianships().length);
    //   this.guardianships().forEach(g => {
    //     console.log(
    //       `Animal ${g.animal.id} → isFavorite: ${g.animal.isFavorite}`
    //     );
    //   });
    // });
  }

  private loadGuardianships() {
    this.guardianshipService.getGuardianships().subscribe({
      next: data => this.rawGuardianships.set(data),
    });
  }

  private loadFavoriteAnimalIds() {
    this.animalSubscriptionService.getFavoriteAnimals().subscribe({
      next: favs => {
        this.favoriteAnimalIds.set(new Set(favs.map(a => a.id)));
      },
      error: () => {
        this.favoriteAnimalIds.set(new Set());
      },
    });
  }

  toDeleteGuardianship(id: string) {
    this.cancelationGuardianshipId.set(id);
    this.showModal.set(true);
  }
  toSubmitCancel($event: boolean) {
    if ($event) {
      this.guardianshipService
        .cancelGuardianship(this.cancelationGuardianshipId())
        .subscribe({
          next: () => this.loadGuardianships(),
          error: err => console.error('Error deleting guardianship:', err),
        });
    }
    this.showModal.set(false);
    this.cancelationGuardianshipId.set('');
  }
  toChangePayment(guardianship: Guardianship) {
    console.log('toChangePayment', guardianship);
    // if (!guardianship.paymentSubscription) this.createPayment(guardianship);
    // else this.renewPayment(guardianship);

    this.createPayment(guardianship);
  }

  renewPayment(guardianship: Guardianship) {
    const id = guardianship.paymentSubscription.id;
    if (!id) return;
    try {
      this.liqpayService.resetSubscription(id).subscribe({
        next: response => {
          const form = document.createElement('form');
          form.method = 'POST';
          form.action = response.gatewayUrl; // → https://www.liqpay.ua/api/3/checkout
          form.style.display = 'none';

          const dataInput = document.createElement('input');
          dataInput.name = 'data';
          dataInput.value = response.data;
          form.appendChild(dataInput);

          const signatureInput = document.createElement('input');
          signatureInput.name = 'signature';
          signatureInput.value = response.signature;
          form.appendChild(signatureInput);

          document.body.appendChild(form);
          form.submit(); // Відкриває LiqPay у тій самій вкладці — ідеально!
        },
        error: err => {
          console.error('LiqPay error:', err);
        },
      });
    } catch (err) {
      console.error(err);
    }
  }

  createPayment(guardianship: Guardianship) {
    try {
      this.liqpayService.startPayment({
        scope: 'guardianship' as PaymentScope,
        isRecurring: true,
        entityId: guardianship.id,
      });
      this.router.navigate(['/payment/details']);
    } catch (err) {
      console.error(err);
    }
  }
  toAdoption(animalId: string) {
    console.log('toAdoption', animalId);
    throw new Error('Method not implemented.');
  }
  toggleFavourite(animalId: string) {
    const isFavorite = this.favoriteAnimalIds().has(animalId);

    if (isFavorite) {
      this.animalSubscriptionService
        .deleteAnimalSubscription(animalId)
        .subscribe({
          next: () => {
            this.favoriteAnimalIds.update(set => {
              const newSet = new Set(set);
              newSet.delete(animalId);
              return newSet;
            });
          },
          error: () => {
            // можна показати тост
          },
        });
    } else {
      this.animalSubscriptionService
        .createAnimalSubscription(animalId)
        .subscribe({
          next: () => {
            this.favoriteAnimalIds.update(set => new Set([...set, animalId]));
          },
          error: () => {
            // можна показати тост
          },
        });
    }
  }

  toProfile() {
    this.router.navigate(['/profile']);
  }
  toAllAnimals() {
    this.router.navigate(['/animals']);
  }
  private calculateAgeParts(birthday: string): [number, number] {
    const today = new Date();
    const birthdate = new Date(birthday);
    const ageInMilliseconds = today.getTime() - birthdate.getTime();
    const ageInDays = Math.floor(ageInMilliseconds / (1000 * 60 * 60 * 24));
    const years = Math.floor(ageInDays / 365);
    const months = Math.floor((ageInDays % 365) / 30);
    return [years, months];
  }
}
