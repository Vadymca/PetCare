import { computed, effect, inject, Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import {
  LiqPayCheckoutRequest,
  LiqPayCheckoutResponse,
  PaymentHistoryResponse,
  PaymentScope,
  PaymentStatusResponse,
  PaymentSubscription,
} from '../models/liqPayCheckoutRequest';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root',
})
export class LiqPayService {
  private api = inject(ApiService);
  private endpoint = `payments/liqpay`;

  private readonly STORAGE_KEY = 'liqpay_checkout_pending';

  private readonly _data = signal<{
    scope: PaymentScope;
    amount?: number;
    description?: string;
    isRecurring?: boolean;
    entityId?: string;
    payerName?: string;
    payerPhone?: string;
    payerEmail?: string;
  } | null>(null);

  private readonly _loading = signal(false);

  readonly data = this._data.asReadonly();
  readonly loading = this._loading.asReadonly();
  readonly scope = computed(() => this._data()?.scope ?? null);

  constructor() {
    // відновлення після F5
    if (typeof window !== 'undefined') {
      const saved = sessionStorage.getItem(this.STORAGE_KEY);
      if (saved) this._data.set(JSON.parse(saved));
    }

    // автозбереження при будь-якій зміні
    effect(() => {
      const data = this._data();
      if (typeof window !== 'undefined') {
        if (data) {
          sessionStorage.setItem(this.STORAGE_KEY, JSON.stringify(data));
        } else {
          sessionStorage.removeItem(this.STORAGE_KEY);
        }
      }
    });
  }

  startPayment(payload: {
    scope: PaymentScope;
    entityId?: string;
    isRecurring?: boolean;
    amount?: number;
    description?: string;
    payerName?: string;
    payerPhone?: string;
    payerEmail?: string;
  }) {
    this.clear();
    this._data.set({
      scope: payload.scope,
      isRecurring: payload.isRecurring ?? false,
      ...(payload.amount !== undefined && { amount: payload.amount }),
      ...(payload.description && { description: payload.description }),
      ...(payload.entityId && { entityId: payload.entityId }),
    });
  }

  update(partial: Partial<Omit<LiqPayCheckoutRequest, 'scope'>>) {
    this._data.update(current => (current ? { ...current, ...partial } : null));
  }

  clear() {
    this._data.set(null);
  }

  proceed(
    overrides?: Partial<LiqPayCheckoutRequest>
  ): Observable<LiqPayCheckoutResponse> {
    const current = this._data();
    if (
      !current?.scope ||
      (current.scope !== 'guardianship' && !current.amount)
    ) {
      console.log('current', current);
      throw new Error('Дані платежу не заповнені');
    }

    const request: LiqPayCheckoutRequest = {
      amount: current.amount,
      scope: current.scope,
      isRecurring: current.isRecurring ?? false,
      ...(current.description && { description: current.description }),
      ...(current.entityId && { entityId: current.entityId }),
      ...(current.payerName && { payerName: current.payerName }),
      ...(current.payerPhone && { payerPhone: current.payerPhone }),
      ...(current.payerEmail && { payerEmail: current.payerEmail }),
      ...overrides,
    };

    this._loading.set(true);
    return this.api
      .post<LiqPayCheckoutResponse>(`${this.endpoint}/checkout`, request)
      .pipe(
        tap({
          next: () => this._loading.set(false),
          error: () => this._loading.set(false),
        })
      );
  }
  // getPaymentStatus(orderId: string) {
  //   const params = new HttpParams().set('orderId', orderId);

  //   return this.api.get<PaymentStatusResponse>(
  //     `${this.endpoint}/status`,
  //     params
  //   );
  // }

  getPaymentStatus(orderId: string) {
    return this.api.getById<PaymentStatusResponse>(`payments/intents`, orderId);
  }

  getPaymentHistory(): Observable<[PaymentHistoryResponse]> {
    return this.api.get<[PaymentHistoryResponse]>(`payments/me/history`);
  }
  // 4. Активні підписки - доопрацювати
  getActiveSubscriptions(): Observable<[PaymentSubscription]> {
    return this.api.get<[PaymentSubscription]>(`payments/me/subscriptions`);
  }
  cancelSubscription(subscriptionId: string) {
    return this.api.post<{ success: boolean; message: string }>(
      `/api/subscriptions/${subscriptionId}/cancel`,
      ''
    );
  }
  resetSubscription(
    subscriptionId: string
  ): Observable<LiqPayCheckoutResponse> {
    return this.api
      .post<LiqPayCheckoutResponse>(
        `/api/subscriptions/${subscriptionId}/reset`,
        ''
      )
      .pipe(
        tap({
          next: () => this._loading.set(false),
          error: () => this._loading.set(false),
        })
      );
  }
}
