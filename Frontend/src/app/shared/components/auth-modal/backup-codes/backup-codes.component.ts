import { UpperCasePipe } from '@angular/common';
import {
  Component,
  EventEmitter,
  inject,
  Input,
  Output,
  signal,
} from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { ModalService } from '../../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-backup-codes',
  standalone: true,
  imports: [
    PrimaryLargeButtonComponent,
    TranslateModule,
    IconComponent,
    UpperCasePipe,
  ],
  templateUrl: './backup-codes.component.html',
  styleUrl: './backup-codes.component.css',
})
export class BackupCodesComponent {
  closeModal() {
    this.modalService.closeModal();
  }
  private modalService = inject(ModalService);

  @Input() loading = signal(false);
  @Input() message = signal<string>('');
  @Output() regenerateCodes = new EventEmitter<void>();
  @Input() codes = signal<string[]>([]);
  @Input() errorMessage = signal<string>('');

  onRegenerateCodes() {
    this.regenerateCodes.emit();
  }

  showMessage(msg: string) {
    this.message.set(msg);
    setTimeout(() => this.message.set(''), 5000); // приховати через 5 сек
  }

  onClose() {
    this.modalService.closeModal();
  }
}
