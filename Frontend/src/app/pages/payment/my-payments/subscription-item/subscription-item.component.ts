import { CommonModule } from '@angular/common';
import { Component, EventEmitter, input, Output } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

import { PaymentSubscription } from '../../../../core/models/liqPayCheckoutRequest';

@Component({
  selector: 'app-subscription-item',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './subscription-item.component.html',
  styleUrl: './subscription-item.component.css',
})
export class SubscriptionItemComponent {
  subscription = input.required<PaymentSubscription>();
  @Output() deleteSubscription = new EventEmitter<void>();

  onDelete() {
    this.deleteSubscription.emit();
  }
}
