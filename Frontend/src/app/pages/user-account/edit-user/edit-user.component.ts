import { isPlatformBrowser, UpperCasePipe } from '@angular/common';
import { Component, effect, inject, PLATFORM_ID, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { filter, map, Observable, of } from 'rxjs';
import { User } from '../../../core/models/user';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/services/auth.service';
import { ModalService } from '../../../core/services/modal.service';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { PrimaryLargeOrangeButtonComponent } from '../../../shared/components/buttons/orange/primary-large-orange-button.component';
import { IconComponent } from '../../../shared/components/icon.component';

@Component({
  selector: 'app-edit-user',
  standalone: true,
  imports: [
    IconComponent,
    TranslateModule,
    UpperCasePipe,
    PrimaryLargeOrangeButtonComponent,
    ReactiveFormsModule,
    PrimaryLargeButtonComponent,
  ],
  templateUrl: './edit-user.component.html',
  styleUrl: './edit-user.component.css',
})
export class EditUserComponent {
  toChangeNotifications() {
    throw new Error('Method not implemented.');
  }
  private authService = inject(AuthService);
  private api = inject(ApiService);
  public user = signal(this.authService._currentUser());
  modalService = inject(ModalService);

  profilePhoto = signal<string | ArrayBuffer | null>(null);
  router = inject(Router);
  platformId = inject(PLATFORM_ID);
  fb = new FormBuilder();
  editForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: [{ value: '', disabled: true }, Validators.required],
    phone: ['', [Validators.required]],
    postalCode: ['', [Validators.required]],
  });
  selectedFile: File | null = null;
  errorMessages = signal<string[]>([]);
  isDisabled = signal(true);
  isLoading = signal(false);

  constructor() {
    effect(() => {
      this.user.set(this.authService._currentUser());
      const userValue = this.user();
      if (userValue) {
        this.profilePhoto.set(userValue.profilePhoto || null);
        this.editForm.patchValue({
          firstName: userValue.firstName,
          lastName: userValue.lastName,
          email: userValue.email,
          phone: userValue.phone,
          postalCode: userValue.postalCode,
        });
      }
    });

    if (isPlatformBrowser(this.platformId)) {
      this.router.events
        .pipe(filter(event => event instanceof NavigationEnd))
        .subscribe(() => {
          window.scrollTo({ top: 0, behavior: 'auto' });
        });
    }
    this.editForm.valueChanges.pipe(takeUntilDestroyed()).subscribe(() => {
      this.isDisabled.set(!this.editForm.valid);
      this.updateErrors();
    });
    // effect(() => {
    //   this.editForm.valueChanges.pipe(takeUntilDestroyed()).subscribe(() => {
    //     this.isDisabled.set(!this.editForm.valid);
    //     this.updateErrors();
    //   });
    // });
  }

  updateErrors() {
    const errors: string[] = [];
    const controls = this.editForm.controls;

    if (controls.firstName.dirty && controls.firstName.invalid) {
      errors.push('FIRST_NAME_REQUIRED');
    }
    if (controls.lastName.dirty && controls.lastName.invalid) {
      errors.push('LAST_NAME_REQUIRED');
    }
    if (controls.postalCode.dirty && controls.postalCode.invalid) {
      errors.push('ZIP_CODE_REQUIRED');
    }
    if (controls.phone.dirty && controls.phone.invalid) {
      if (controls.phone.errors?.['required']) {
        errors.push('PHONE_REQUIRED');
      }
      if (controls.phone.errors?.['pattern']) {
        errors.push('PHONE_INVALID');
      }
    }

    this.errorMessages.set(errors);
  }
  onSubmit() {
    if (this.editForm.invalid) {
      this.editForm.markAllAsTouched();
      return;
    }

    this.errorMessages.set([]);

    this.handleSubmitEditForm();
  }
  handleSubmitEditForm() {
    this.isLoading.set(true);
    const firstName = this.editForm.value.firstName;
    const lastName = this.editForm.value.lastName;
    const phone = this.editForm.value.phone;
    const postalCode = this.editForm.value.postalCode;
    const currentUser = this.user();
    if (!firstName || !lastName || !phone || !postalCode) return;
    if (!currentUser) return;
    this.isLoading.set(true);
    const changedFields: Partial<User> = {};

    if (firstName !== currentUser.firstName)
      changedFields.firstName = firstName;
    if (lastName !== currentUser.lastName) changedFields.lastName = lastName;
    if (phone !== currentUser.phone) changedFields.phone = phone;
    if (postalCode !== currentUser.postalCode)
      changedFields.postalCode = postalCode;

    let upload$: Observable<Partial<User>>;

    if (this.selectedFile) {
      // файл завантажуємо на сервер і отримуємо URL
      upload$ = this.api
        .uploadFile<{ url: string }>('media/upload', this.selectedFile)
        .pipe(
          map(response => {
            changedFields.profilePhoto = response.url;
            return changedFields;
          })
        );
    } else {
      // немає файлу — просто беремо уже зібрані зміни
      upload$ = of(changedFields);
    }

    upload$.subscribe({
      next: finalFields => {
        if (Object.keys(finalFields).length === 0) {
          this.isLoading.set(false);
          return;
        }

        this.authService.updateUser(finalFields).subscribe({
          next: () => this.router.navigate(['/profile']),
          error: err => {
            console.error('Error updating user:', err);
            this.errorMessages.set(['ERROR.500_MESSAGE']);
          },
        });
        this.isLoading.set(false);
      },
      error: err => {
        console.error('Error uploading photo:', err);
        this.errorMessages.set(err.message);
        this.isLoading.set(false);
      },
    });
  }

  toChangePassword() {
    this.modalService.openModal('change-password');
  }
  toProfile() {
    this.router.navigate(['/profile']);
  }
  toSetTwoFA() {
    this.router.navigate(['/profile/security']);
  }
  onPhotoSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input?.files?.length) {
      const file = input.files[0];
      this.selectedFile = file; // нове поле для збереження File
      const reader = new FileReader();
      reader.onload = () => this.profilePhoto.set(reader.result);
      reader.readAsDataURL(file);
    }
  }
  get firstNameInvalid() {
    return (
      this.editForm.controls.firstName.touched &&
      this.editForm.controls.firstName.invalid
    );
  }
  get lastNameInvalid() {
    return (
      this.editForm.controls.lastName.touched &&
      this.editForm.controls.lastName.invalid
    );
  }
  get postalCodeInvalid() {
    return (
      this.editForm.controls.postalCode.touched &&
      this.editForm.controls.postalCode.invalid
    );
  }
  get phoneInvalid() {
    return (
      this.editForm.controls.phone.touched &&
      this.editForm.controls.phone.invalid
    );
  }
}
