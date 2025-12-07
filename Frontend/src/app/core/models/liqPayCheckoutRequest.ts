import { AnimalAidRequest } from './animalAidRequest';
import { Guardianship } from './guardianship';

export type PaymentScope = 'global' | 'aidRequest' | 'guardianship';

export interface LiqPayCheckoutRequest {
  amount?: number;
  description?: string;
  isRecurring?: boolean;
  scope: PaymentScope;
  entityId?: string; // id запиту на допомогу або id опіки
  payerName?: string;
  payerPhone?: string;
  payerEmail?: string;
}

export interface LiqPayCheckoutResponse {
  data: string;
  signature: string;
  publicKey: string;
  gatewayUrl: string;
  orderId: string;
  resultUrl: string;
  // інші поля, які повертає бекенд
}
export interface PaymentStatusResponse {
  orderId: string;
  status: PaymentStatus; //перевірити
  success: boolean;
  providerPaymentId?: string;
  scope?: PaymentScope;
  scopeId?: string;
  userId?: string;
  amount: number;
  currency: 'UAH';
  isRecurring: boolean;
  anonymous?: boolean;
  donation: Partial<Payment>;
  guardianship?: Partial<Guardianship>;
  animalAidRequest?: Partial<AnimalAidRequest>; //додала
  subscription?: Partial<PaymentSubscription>;
  createdAt?: string;
  updatedAt?: string;
  message?: string;
}

export type PaymentStatus = 'pending' | 'success' | 'failure';
export type PaymentSubscriptionStatus =
  | 'RequiresPayment'
  | 'Active'
  | 'Completed';
// | 'in_progress'
// | 'reversed'
// | 'expired'
export interface Payment {
  id: string; // внутрішній ID (наприклад: ORD-2025-11-123)
  amount: number;
  currency: 'UAH';
  purpose?: string;
  status: PaymentStatus;
  transactionId: string; // payment_id від LiqPay (наприклад: 1234567890)
  targetEntityId?: string;
  //targetEntity?: або проект або опіка
  orderId?: string; // старий orderId, якщо був (можна задепрекейтити)

  isRecurring: boolean;
  scope: PaymentScope;
  entityId: string; // ID тварини, заявки тощо
  description: string;
  payerEmail?: string;
  payerName?: string;
  payerPhone?: string;
  isAnonymous: boolean;
  createdAt: string; // ISO string
  completedAt?: string;
  nextPaymentDate?: string; // тільки для recurring
  cancelledAt?: string;
}
/** Список платежів / історія */
export interface PaymentHistoryResponse {
  id: string;
  providerSubscriptionId?: string;
  amount: number;
  currency: 'UAH';
  status: PaymentStatus;
  recurring: boolean;
  purpose: string;
  targetEntityId?: string;
  donationDate: string;
}

export interface PaymentSubscription {
  id: string;

  amount: number;
  currency: 'UAH';
  provider?: string; //це що?
  providerSubscriptionId?: string; //нема такого
  status: PaymentSubscriptionStatus;
  guardianship?: Guardianship;
  aidRequest?: AnimalAidRequest;
  scopeType: PaymentScope;
  scopeId?: string;
  createdAt: string; //"2025-12-04T20:25:42.841Z"
  lastChargeAt: string;
  nextChargeAt: string;
  purpose?: string;
}
