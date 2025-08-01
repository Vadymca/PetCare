import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
@Component({
  selector: 'app-acces-denied',
  standalone: true,
  imports: [TranslateModule],
  templateUrl: './acces-denied.component.html',
  styleUrl: './acces-denied.component.css',
})
export class AccesDeniedComponent {
  private router = inject(Router);
  goHome() {
    this.router.navigate(['/']);
  }
}
