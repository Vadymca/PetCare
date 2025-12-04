import { UpperCasePipe } from '@angular/common';
import { Component, signal } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { FavoriteAnimalsComponent } from './favorite-animals/favorite-animals.component';
import { FavoriteSheltersComponent } from './favorite-shelters/favorite-shelters.component';

@Component({
  selector: 'app-favourites',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    FavoriteAnimalsComponent,
    FavoriteSheltersComponent,
  ],
  templateUrl: './favorites.component.html',
})
export class FavoritesComponent {
  isAnimalsVisible = signal(true);
  onSheltersClick() {
    this.isAnimalsVisible.set(false);
  }
  onAnimalsClick() {
    this.isAnimalsVisible.set(true);
  }
}
