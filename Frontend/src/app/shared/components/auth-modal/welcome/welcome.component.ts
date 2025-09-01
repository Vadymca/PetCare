import { UpperCasePipe } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { ModalState } from '../../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { SecondaryLargeButtonComponent } from '../../buttons/blue/secondary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-welcome',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    IconComponent,
    PrimaryLargeButtonComponent,
    SecondaryLargeButtonComponent,
  ],
  templateUrl: './welcome.component.html',
  styleUrl: './welcome.component.css',
})
export class WelcomeComponent {
  @Output() selectOption = new EventEmitter<ModalState['component']>();

  emitOption(option: ModalState['component']) {
    console.log('WelcomeComponent: Selected option:', option); // Лог для дебагу
    this.selectOption.emit(option);
  }
}
