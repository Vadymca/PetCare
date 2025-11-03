import { UpperCasePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { SecondaryLargeButtonComponent } from '../../../shared/components/buttons/blue/secondary-large-button.component';
import { HomeNewsComponent } from '../../../shared/components/home-news/home-news.component';
import { HomePartnersComponent } from '../../../shared/components/home-partners/home-partners.component';
import { IconComponent } from '../../../shared/components/icon.component';

@Component({
  selector: 'app-news',
  standalone: true,
  imports: [
    TranslateModule,
    UpperCasePipe,
    IconComponent,
    HomeNewsComponent,
    HomePartnersComponent,

    SecondaryLargeButtonComponent,
  ],
  templateUrl: './news.component.html',
  styleUrl: './news.component.css',
})
export class NewsComponent {
  router = inject(Router);
  callSupportLine() {
    this.router.navigate(['/feedback-form']);
  }
}
