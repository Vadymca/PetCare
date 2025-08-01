import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-internal-server-error',
	standalone: true,
  imports: [TranslateModule],
  templateUrl: './internal-server-error.component.html',
  styleUrl: './internal-server-error.component.css',
})
export class InternalServerErrorComponent {
  private router = inject(Router);
  goHome() {
    this.router.navigate(['/']);
  }
}
