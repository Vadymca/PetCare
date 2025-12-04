import { UpperCasePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { PaymentStatusResponse } from '../../../core/models/liqPayCheckoutRequest';
import { LiqPayService } from '../../../core/services/liq-pay-service.service';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { IconComponent } from '../../../shared/components/icon.component';
import { LoadingSpinnerComponent } from '../../../shared/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-payment-status',
  standalone: true,
  imports: [
    IconComponent,
    TranslateModule,
    LoadingSpinnerComponent,
    PrimaryLargeButtonComponent,
    UpperCasePipe,
  ],
  templateUrl: './payment-status.component.html',
  styleUrl: './payment-status.component.css',
})
export class PaymentStatusComponent {
  goToDonations() {
    this.router.navigate(['payment/amount']);
  }
  goToProjects() {
    this.router.navigate(['/animal-aid-requests']);
  }
  goToGuardianships() {
    this.router.navigate(['guardianships']);
  }
  private route = inject(ActivatedRoute);
  router = inject(Router);
  private liqPayService = inject(LiqPayService);
  paymentStatus = signal<PaymentStatusResponse | null>(null);
  isLoading = signal<boolean>(true);
  isError = signal<boolean>(false);

  constructor() {
    const orderId = this.route.snapshot.queryParams['orderId'];

    if (!orderId) {
      this.isLoading.set(false);
      this.router.navigate(['service-unavailable']);
    }

    this.getPaymentStatus(orderId);
  }
  getPaymentStatus(paymentId: string) {
    try {
      this.liqPayService.getPaymentStatus(paymentId).subscribe(data => {
        this.paymentStatus.set(data);
      });
    } catch (error) {
      this.isError.set(true);
      console.error(error);
    }

    this.isLoading.set(false);
  }
}
