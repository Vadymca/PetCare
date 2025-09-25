import { CommonModule } from '@angular/common';
import {
  Component,
  effect,
  EventEmitter,
  Input,
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
export class FinancialSupportComponent {
  @Input() sums: number[] = [50, 100, 200, 500, 1000];
  @Output() selectedSum = new EventEmitter<number>();
  @Output() selectedPeriod = new EventEmitter<boolean>();
  chosenSum = signal<number | undefined>(undefined);
  customSum = signal<number | undefined>(undefined);
  donateOnce = signal(true);
  fb = new FormBuilder();
  registerForm = this.fb.group({
    customSum: ['', [Validators.required]],
  });
  constructor() {
    this.donateOnce.set(true);
    effect(() => {
      // Тут беремо значення форми через signal-обгортку
      this.registerForm.valueChanges.subscribe(() => {
        this.customSum.set(
          parseInt(this.registerForm.value.customSum?.toString() || '0', 10)
        );
        if (this.customSum() && this.customSum()! > 0) {
          this.chosenSum.set(undefined);
        }
      });
    });
  }
  
  selectSum(sum: number) {
    this.chosenSum.set(sum);

    this.customSum.set(undefined); // очистити кастомну суму
  }
  onCustomSumChange() {
    const num = parseInt(
      this.registerForm.value.customSum?.toString() || '0',
      10
    );
    if (!isNaN(num) && num > 0) {
      this.customSum.set(num);
      this.chosenSum.set(undefined); // очистити вибрану суму
    } else {
      this.customSum.set(undefined);
    }
  }

  onContinueClick() {
    const sumToSend = this.chosenSum() ?? this.customSum();

    if (sumToSend) {
      this.selectedSum.emit(sumToSend);
    }
    this.selectedPeriod.emit(this.donateOnce());
  }

  onDonateMontlyClick() {
    this.donateOnce.set(false);
  }
  onDonateOnceClick() {
    this.donateOnce.set(true);
  }
}
