import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { IconComponent } from '../../../shared/components/icon.component';
@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [TranslateModule, IconComponent, PrimaryLargeButtonComponent],
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.css',
})
export class NotFoundComponent {
  private router = inject(Router);
  goHome() {
    this.router.navigate(['/']);
  }
}
