import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-auth-buttons',
  standalone: true,
  imports: [TranslateModule],
  template: `
    <button
      (click)="goToLogin()"
      class="bg-gray-600 text-white px-4 py-2 rounded hover:bg-gray-700"
    >
      {{ 'LOGIN_OR_REGISTER' | translate }}
    </button>
  `,
})
export class AuthButtonsComponent {
  router = inject(Router);

  goToLogin() {
    this.router.navigate(['/login']);
  }
}
