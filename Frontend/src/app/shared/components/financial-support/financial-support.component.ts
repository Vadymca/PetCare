import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  signal,
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { SecondaryLargeButtonComponent } from '../buttons/blue/secondary-large-button.component';
import { IconComponent } from '../icon.component';

@Component({
  selector: 'app-financial-support',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    IconComponent,
    SecondaryLargeButtonComponent,
    ReactiveFormsModule,
  ],
  templateUrl: './financial-support.component.html',
  styleUrl: './financial-support.component.css',
})
export class FinancialSupportComponent implements OnInit {
  @Input() initialSum: number | null = null;
  @Input() initialIsOnce = true;
  @Input() sums: number[] = [50, 100, 200, 500, 1000];
  @Output() selectionConfirmed = new EventEmitter<{
    amount: number;
    isOnce: boolean; // true = разово, false = щомісяця
  }>();
  chosenSum = signal<number | null>(null);
  customSum = signal<number | null>(null);
  donateOnce = signal(true);
  fb = new FormBuilder();
  registerForm = this.fb.group({
    customSum: ['', [Validators.required]],
  });
  ngOnInit() {
    this.donateOnce.set(this.initialIsOnce);

    if (this.initialSum !== null && this.initialSum > 0) {
      // Якщо сума є в стандартному списку — підсвічуємо кнопку
      if (this.sums.includes(this.initialSum)) {
        this.chosenSum.set(this.initialSum);
        this.customSum.set(null);
        this.registerForm.patchValue({ customSum: '' });
      }
      // Інакше — це кастомна сума → кладемо в інпут
      else {
        this.customSum.set(this.initialSum);
        this.chosenSum.set(null);
        this.registerForm.patchValue({ customSum: this.initialSum.toString() });
      }
    }
  }

  constructor() {
    this.registerForm.valueChanges.subscribe(value => {
      const input = (value.customSum || '').toString().trim();
      const num = parseInt(input, 10);

      if (!isNaN(num) && num > 0) {
        this.customSum.set(num);
        this.chosenSum.set(null);
      } else {
        this.customSum.set(null);
      }
    });
  }
  selectSum(sum: number) {
    this.chosenSum.set(sum);

    this.customSum.set(null); // очистити кастомну суму
    this.registerForm.patchValue({ customSum: '' });
  }
  onCustomSumChange() {
    const num = parseInt(
      this.registerForm.value.customSum?.toString() || '0',
      10
    );
    if (!isNaN(num) && num > 0) {
      this.customSum.set(num);
      this.chosenSum.set(null); // очистити вибрану суму
    } else {
      this.customSum.set(null);
    }
  }

  onContinueClick() {
    const amount = this.chosenSum() ?? this.customSum();

    if (amount && amount > 0) {
      this.selectionConfirmed.emit({
        amount,
        isOnce: this.donateOnce(), // true = разово
      });
    }
  }

  onDonateMontlyClick() {
    this.donateOnce.set(false);
  }
  onDonateOnceClick() {
    this.donateOnce.set(true);
  }
}
