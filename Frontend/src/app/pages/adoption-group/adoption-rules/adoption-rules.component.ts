import { UpperCasePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { SecondaryLargeButtonComponent } from '../../../shared/components/buttons/blue/secondary-large-button.component';
import { IconComponent } from '../../../shared/components/icon.component';

@Component({
  selector: 'app-adoption-rules',
  standalone: true,
  imports: [
    IconComponent,
    TranslateModule,
    UpperCasePipe,
    SecondaryLargeButtonComponent,
  ],
  templateUrl: './adoption-rules.component.html',
  styleUrl: './adoption-rules.component.css',
})
export class AdoptionRulesComponent {
  router = inject(Router);
  backBottomClick() {
    this.router.navigate(['adoption']);
  }
}
