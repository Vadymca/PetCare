import { Component, effect, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { take } from 'rxjs';
import { HeaderComponent } from './core/header/header.component';
import { AuthService } from './core/services/auth.service';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  private authService = inject(AuthService);
  title = 'petcare-frontend';
  constructor() {
    effect(() => {
      this.authService
        .refreshToken()
        .pipe(take(1))
        .subscribe({
          next: () => {
            console.log('Токен оновлено(app.component)');
          },
          error: err => {
            console.error(
              'Помилка при спробі оновлення токена (app.component):',
              err
            );
          },
        });
    });
  }
}
