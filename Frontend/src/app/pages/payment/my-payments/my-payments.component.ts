import { UpperCasePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import {
  PaymentHistoryResponse,
  PaymentSubscription,
} from '../../../core/models/liqPayCheckoutRequest';
import { LiqPayService } from '../../../core/services/liq-pay-service.service';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { ConfirmModalComponent } from '../../../shared/components/confirm-modal/confirm-modal.component';
import { IconComponent } from '../../../shared/components/icon.component';
import { PaymentItemComponent } from './payment-item/payment-item.component';
import { SubscriptionItemComponent } from './subscription-item/subscription-item.component';

@Component({
  selector: 'app-my-payments',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    IconComponent,
    SubscriptionItemComponent,
    PaymentItemComponent,
    PrimaryLargeButtonComponent,
    ConfirmModalComponent,
  ],
  templateUrl: './my-payments.component.html',
  styleUrl: './my-payments.component.css',
})
export class MyPaymentsComponent {
  router = inject(Router);
  liqPayService = inject(LiqPayService);
  payments = signal<PaymentHistoryResponse[]>([]);
  subscriptions = signal<PaymentSubscription[]>([]);
  showModalWindow = signal(false);
  subscriptionToDelete = signal<PaymentSubscription | null>(null);
  constructor() {
    this.loadPaymentArchive();
    this.loadPaymentSubscriptions();
  }
  toProfile() {
    this.router.navigate(['profile']);
  }
  private loadPaymentArchive() {
    try {
      this.liqPayService.getPaymentHistory().subscribe(result => {
        this.payments.set(result);
      });
    } catch {
      this.payments.set([]);
    }
  }
  private loadPaymentSubscriptions() {
    try {
      this.liqPayService.getActiveSubscriptions().subscribe(result => {
        this.subscriptions.set(result);
      });
    } catch {
      this.subscriptions.set([]);
    }
  }
  cancelSubscription() {
    const data = this.subscriptionToDelete();
    console.log(data);
    if (!data || !data.providerSubscriptionId) {
      this.showModalWindow.set(false);
      return;
    }
    this.showModalWindow.set(false);

    this.liqPayService
      .cancelSubscription(data.providerSubscriptionId)
      .subscribe(() => {
        this.loadPaymentSubscriptions();
      });
  }
  showModal(subscription: PaymentSubscription) {
    this.showModalWindow.set(true);
    this.subscriptionToDelete.set(subscription);
  }
  goToDonations() {
    this.router.navigate(['payment/amount']);
  }
}
