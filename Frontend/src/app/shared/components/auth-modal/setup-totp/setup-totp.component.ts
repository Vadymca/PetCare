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
import { PrimaryLargeOrangeButtonComponent } from '../../buttons/orange/primary-large-orange-button.component';

@Component({
  selector: 'app-setup-totp',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    TranslateModule,
    PrimaryLargeOrangeButtonComponent,
  ],
  templateUrl: './setup-totp.component.html',
  styleUrl: './setup-totp.component.css',
})
export class SetupTotpComponent implements OnInit {
  @Input() loading = signal(false);
  @Input() errorMessage = signal('');
  @Input() qrCodeImage = signal('');
  @Input() manualKey = signal('');

  @Output() submitForm = new EventEmitter<string>();

  fb = new FormBuilder();
  totpForm = this.fb.group({
    code: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
  });

  isDisabled = signal(true);

  ngOnInit(): void {
    this.totpForm.valueChanges.subscribe(() => {
      this.isDisabled.set(!this.totpForm.valid || this.loading());
    });
  }

  onSubmit() {
    if (this.totpForm.invalid) return;
    if (!this.totpForm.value.code) return;
    this.submitForm.emit(this.totpForm.value.code);
  }
}
