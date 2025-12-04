import { Animal } from './animal';
import { PaymentSubscription } from './liqPayCheckoutRequest';

export interface Guardianship {
  id: string;
  startDate: string;
  graceUntil: string;
  status: GuardianshipStatus;
  animal: Animal;
  paymentSubscription: Partial<PaymentSubscription>;
  animalName?: string;
}

export type GuardianshipStatus = 'Active' | 'RequiresPayment' | 'Completed';
