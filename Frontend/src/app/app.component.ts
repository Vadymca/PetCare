import { isPlatformBrowser } from '@angular/common';
import { Component, inject, OnInit, PLATFORM_ID } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { take } from 'rxjs';
import { FooterComponent } from './core/footer/footer.component';
import { HeaderComponent } from './core/header/header.component';
import { AuthService } from './core/services/auth.service';
import { AuthModalComponent } from './shared/components/auth-modal/auth-modal/auth-modal.component';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, AuthModalComponent, FooterComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  private authService = inject(AuthService);
  private platformId = inject(PLATFORM_ID);
  title = 'petcare-frontend';
  ngOnInit() {
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }
    this.authService
      .refreshToken()
      .pipe(take(1))
      .subscribe({
        next: () => console.log('Токен оновлено(app.component)'),
        error: () =>
          console.error('Помилка при спробі оновлення токена (app.component)'),
      });
  }
}
