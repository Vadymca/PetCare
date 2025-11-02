import { UpperCasePipe } from '@angular/common';
import {
  Component,
  EventEmitter,
  OnDestroy,
  Output,
  signal,
} from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

import { ModalState } from '../../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-registration-confirmation',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    PrimaryLargeButtonComponent,
    ReactiveFormsModule,
    RouterModule,
    IconComponent,
  ],
  templateUrl: './registration-confirmation.component.html',
  styleUrl: './registration-confirmation.component.css',
})
export class RegistrationConfirmationComponent implements OnDestroy {
  @Output() selectOption = new EventEmitter<ModalState['component']>();
  @Output() resendVerificationEmail = new EventEmitter<void>();

  resendTimer = signal(0);
  private intervalId: number | null = null;
  emitOption(option: ModalState['component']) {
    this.selectOption.emit(option);
  }
  emitResendVerificationEmail() {
    if (this.resendTimer() > 0) return; // поки таймер працює, не дозволяємо клік
    this.resendVerificationEmail.emit();
    this.startResendTimer();
  }
  private startResendTimer() {
    this.resendTimer.set(30); // 30 секунд
    this.intervalId = window.setInterval(() => {
      this.resendTimer.update(v => v - 1);
      if (this.resendTimer() <= 0) {
        if (this.intervalId !== null) {
          clearInterval(this.intervalId);
          this.intervalId = null; // обнуляємо після очищення
        }
      }
    }, 1000);
  }

  ngOnDestroy(): void {
    if (this.intervalId !== null) {
      clearInterval(this.intervalId);
      this.intervalId = null;
    }
  }
}
