import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { IconComponent } from '../../../shared/components/icon.component';

@Component({
  selector: 'app-service-unavailable',
  standalone: true,
  imports: [TranslateModule, PrimaryLargeButtonComponent, IconComponent],
  templateUrl: './service-unavailable.component.html',
  styleUrl: './service-unavailable.component.css',
})
export class ServiceUnavailableComponent {
  private router = inject(Router);
  goHome() {
    this.router.navigate(['/']);
  }
}
