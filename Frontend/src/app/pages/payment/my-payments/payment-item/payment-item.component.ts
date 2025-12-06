import { CommonModule } from '@angular/common';
import { Component, input } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PaymentHistoryResponse } from '../../../../core/models/liqPayCheckoutRequest';

@Component({
  selector: 'app-payment-item',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './payment-item.component.html',
  styleUrl: './payment-item.component.css',
})
export class PaymentItemComponent {
  payment = input.required<PaymentHistoryResponse>();
}
