import { CommonModule } from '@angular/common';
import { Component, effect, inject, signal } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { AnimalSubscription } from '../../core/models/animalSubscription';
import { ShelterSubscription } from '../../core/models/shelterSubscriptions';
import { AnimalSubscriptionService } from '../../core/services/animal-subscription.service';
import { AuthService } from '../../core/services/auth.service';
import { ShelterSubscriptionService } from '../../core/services/shelter-subscription.service';
import { AnimalSubscriptionCardComponent } from '../../shared/components/animal-subscription-card.component';
import { ShelterSubscriptionCardComponent } from '../../shared/components/shelter-subscription-card';
export const passwordsMatchValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const password = control.get('password')?.value;
  const confirmPassword = control.get('confirmPassword')?.value;
  return password === confirmPassword ? null : { passwordsMismatch: true };
};
@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TranslateModule,
    AnimalSubscriptionCardComponent,
    ShelterSubscriptionCardComponent,
  ],
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css'],
})
export class UserProfileComponent {
  // Сигнал для зображення
  private authService = inject(AuthService);
  private animalSubscriptionService = inject(AnimalSubscriptionService);
  private shelterSubscriptionService = inject(ShelterSubscriptionService);
  public user = signal(this.authService.currentUser());
  profilePhoto = signal<string | ArrayBuffer | null>(null);

  fb = inject(FormBuilder);
  // Форма
  profileForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phone: [
      '',
      [
        Validators.required,
        Validators.pattern(/^(?=(?:.*\d){5,})[\d+\-\s()]+$/),
      ],
    ],
  });
  editProfileSubmitted = signal(false);
  changePasswordForm = this.fb.group(
    {
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
    },
    { validators: passwordsMatchValidator } // це груповий валідатор
  );

  changePasswordSubmitted = signal(false);
  showPassword = signal(false);
  showRepeatPassword = signal(false);
  successChangePasswordMessage = signal(false);
  errorChangePasswordMessage = signal(false);

  animalSubscriptions = signal<AnimalSubscription[]>([]);
  shelterSubscriptions = signal<ShelterSubscription[]>([]);
  constructor() {
    effect(() => {
      const userValue = this.user();
      if (userValue) {
        this.profileForm.patchValue({
          firstName: userValue.firstName,
          lastName: userValue.lastName,
          email: userValue.email,
          phone: userValue.phone,
        });
        this.profilePhoto.set(userValue.profilePhoto || null);
        this.changePasswordForm.patchValue({
          password: '',
          confirmPassword: '',
        });
        console.log('userValue.id ' + userValue.id);
        this.animalSubscriptionService
          //.getAnimalSubscriptions()
          .getAnimalSubscriptionsByUserId(userValue.id)
          .subscribe(animalSubscriptions => {
            console.log(animalSubscriptions);
            this.animalSubscriptions.set(animalSubscriptions);
            console.log(
              'animalSubscriptions.length ' + this.animalSubscriptions().length
            );
          });
        this.shelterSubscriptionService
          .getShelterSubscriptionsByUserId(userValue.id)
          .subscribe(shelterSubscriptions => {
            console.log(shelterSubscriptions);
            this.shelterSubscriptions.set(shelterSubscriptions);
            console.log(
              'shelterSubscriptions.length ' +
                this.shelterSubscriptions().length
            );
          });
      }
    });
  }

  togglePasswordVisibility() {
    this.showPassword.update(v => !v);
  }

  toggleRepeatPasswordVisibility() {
    this.showRepeatPassword.update(v => !v);
  }
  // Метод завантаження фото
  onPhotoSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input?.files?.length) {
      const file = input.files[0];
      const reader = new FileReader();
      reader.onload = () => this.profilePhoto.set(reader.result);
      reader.readAsDataURL(file);
    }
  }

  // Валідація пароля
  get passwordsMatch(): boolean {
    const { password, confirmPassword } = this.changePasswordForm.value;
    return this.changePasswordForm.touched && password === confirmPassword;
  }

  // Обробка відправки
  onSubmit(): void {
    this.editProfileSubmitted.set(true);
    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      return;
    }

    const updatedData = {
      ...this.profileForm.value,
      photo: this.profilePhoto(),
    };

    console.log('Дані для збереження:', updatedData);
    // Тут виклик API для збереження даних
  }
  onChengePassword(): void {
    this.changePasswordSubmitted.set(true);
    if (this.changePasswordForm.invalid) {
      this.changePasswordForm.markAllAsTouched();
      return;
    }
    console.log('Дані для зміни пароля:', this.changePasswordForm.value);
    if (!this.passwordsMatch) {
      alert('Паролі не збігаються');
      return;
    }
    const newPassword = this.changePasswordForm.value.password;
    if (newPassword) {
      this.authService.changePassword(newPassword).subscribe({
        next: () => {
          this.successChangePasswordMessage.set(true);
          this.changePasswordForm.reset();
          console.log('Пароль успішно змінено');
        },
        error: err => {
          alert('помилка з сервера');
          this.errorChangePasswordMessage.set(true);
          console.error('Помилка при зміні пароля:', err);
        },
      });
      this.changePasswordSubmitted.set(false);
    }
  }

  deleteAnimalSubscription(animalSubscriptionId: string): void {
    this.animalSubscriptionService
      .deleteAnimalSubscription(animalSubscriptionId)
      .subscribe({
        next: () => {
          const updated = this.animalSubscriptions().filter(
            sub => sub.id !== animalSubscriptionId
          );
          this.animalSubscriptions.set(updated);
        },
        error: err => {
          console.error('Error deleting animal subscription:', err);
        },
      });
  }
  deleteShelterSubscription(shelterSubscriptionId: string): void {
    this.shelterSubscriptionService
      .deleteShelterSubscription(shelterSubscriptionId)
      .subscribe({
        next: () => {
          const updated = this.shelterSubscriptions().filter(
            sub => sub.id !== shelterSubscriptionId
          );
          this.shelterSubscriptions.set(updated);
        },
        error: err => {
          console.error('Error deleting shelter subscription:', err);
        },
      });
  }
}
