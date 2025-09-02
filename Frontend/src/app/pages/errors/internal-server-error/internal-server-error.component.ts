import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { PrimaryLargeButtonComponent } from "../../../shared/components/buttons/blue/primary-large-button.component";
import { IconComponent } from "../../../shared/components/icon.component";

@Component({
  selector: 'app-internal-server-error',
	standalone: true,
  imports: [TranslateModule, PrimaryLargeButtonComponent, IconComponent],
  templateUrl: './internal-server-error.component.html',
  styleUrl: './internal-server-error.component.css',
})
export class InternalServerErrorComponent {
  private router = inject(Router);
  goHome() {
    this.router.navigate(['/']);
  }
}
