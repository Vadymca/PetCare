import { UpperCasePipe } from '@angular/common';
import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
  signal,
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from '../../../../core/services/auth.service';
import { ModalService } from '../../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../../buttons/blue/primary-large-button.component';
import { IconComponent } from '../../icon.component';

@Component({
  selector: 'app-backup-code-login',
  standalone: true,
  imports: [
    PrimaryLargeButtonComponent,
    TranslateModule,
    ReactiveFormsModule,
    UpperCasePipe,
    IconComponent,
  ],
  templateUrl: './backup-code-login.component.html',
  styleUrl: './backup-code-login.component.css',
})
export class BackupCodeLoginComponent implements OnInit {
  private auth = inject(AuthService);

  fb = new FormBuilder();
  modal = inject(ModalService);
  backupForm = this.fb.group({
    code: ['', [Validators.required]],
  });
  isDisabled = signal(true);
  @Input() loading = signal(false);
  @Input() errorMessage = signal<string>('');
  @Output() submitForm = new EventEmitter<string>();

  submitCode() {
    if (this.backupForm.valid) {
      if (this.backupForm.controls.code.value) {
        this.submitForm.emit(this.backupForm.controls.code.value.toUpperCase());
      }
    }
  }
  ngOnInit() {
    this.backupForm.valueChanges.subscribe(() => {
      this.isDisabled.set(!this.backupForm.valid || this.loading());
    });
  }
  get codeInvalid() {
    return (
      this.backupForm.controls.code.touched &&
      this.backupForm.controls.code.invalid
    );
  }
  toTwoFactor() {
    this.modal.openModal('two-factor');
  }
}
