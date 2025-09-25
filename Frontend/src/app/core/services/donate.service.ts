import { inject, Injectable, signal, WritableSignal } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root',
})
export class DonateService {
  private api = inject(ApiService);
  private endpoint = `liqpay`;
  private donationData: WritableSignal<{
    animalAidRequestId?: string | null;
    animalCareId?: string | null;
    amount?: number;
    fullName?: string | null;
    paymentType?: 'one-time' | 'subscription' | null;
  }> = signal({});
  setDonationData(data: {
    animalAidRequestId?: string;
    animalCareId?: string;
    amount?: number;
    fullName?: string;
    paymentType?: 'one-time' | 'subscription';
  }) {
    console.log('Вставлені такі дані: ', data);
    console.log('donationData: ', this.donationData);
    this.donationData.set(data);
  }
  getDonationData() {
    return this.donationData.asReadonly();
  }

  clearDonationData() {
    this.donationData.set({});
  }
  private _createOneTimePayment(
    animalAidRequestId: string | null,
    animalCareId: string | null,
    amount: number,
    fullName: string | null
  ): Observable<{ data: string; signature: string }> {
    return this.api.post<{ data: string; signature: string }>(
      `${this.endpoint}/create-payment`,
      {
        animalAidRequestId,
        animalCareId,
        amount,
        fullName,
        paymentType: 'one-time',
      }
    );
  }
  createOneTimePayment(): Observable<{ data: string; signature: string }> {
    const data = this.donationData();
    // Перевірка обов’язкових полів
    if (data.amount === undefined || data.amount <= 0) {
      return throwError(
        () =>
          new Error(
            'Amount is required and must be greater than 0 for one-time payment'
          )
      );
    }
    // Встановлення null для необов’язкових полів
    const animalAidRequestId = data.animalAidRequestId || null;
    const animalCareId = data.animalCareId || null;
    const fullName = data.fullName || null;

    return this._createOneTimePayment(
      animalAidRequestId,
      animalCareId,
      data.amount,
      fullName
    );
  }
  private _createSubscription(
    animalAidRequestId: string | null,
    animalCareId: string | null,
    amount: number,
    fullName: string | null
  ): Observable<{ data: string; signature: string }> {
    return this.api.post<{ data: string; signature: string }>(
      `${this.endpoint}/create-payment`,
      {
        animalAidRequestId,
        animalCareId,
        amount,
        fullName,
        paymentType: 'subscription',
      }
    );
  }
  createSubscription(): Observable<{ data: string; signature: string }> {
    const data = this.donationData();
    if (data.amount === undefined || data.amount <= 0) {
      return throwError(
        () =>
          new Error(
            'Amount is required and must be greater than 0 for subscription'
          )
      );
    }
    const animalAidRequestId = data.animalAidRequestId || null;
    const animalCareId = data.animalCareId || null;
    const fullName = data.fullName || null;

    return this._createSubscription(
      animalAidRequestId,
      animalCareId,
      data.amount,
      fullName
    );
  }

  createPaymentForm({ data, signature }: { data: string; signature: string }) {
    return `
      <form id="liqpay-form" method="POST" action="https://www.liqpay.ua/api/3/checkout" accept-charset="utf-8" style="display: none;">
        <input type="hidden" name="data" value="${data}" />
        <input type="hidden" name="signature" value="${signature}" />
      </form>
    `;
  }
}
