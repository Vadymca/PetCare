import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-service-unavailable',
  standalone: true,
  imports: [TranslateModule],
  templateUrl: './service-unavailable.component.html',
  styleUrl: './service-unavailable.component.css',
})
export class ServiceUnavailableComponent {
  private router = inject(Router);
  goHome() {
    this.router.navigate(['/']);
  }
}
