import { CommonModule } from '@angular/common';
import { Component, effect, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Meta, Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { filter, switchMap } from 'rxjs';
import { LostPet } from '../../../core/models/lostPet';
import { LostPetService } from '../../../core/services/lost-pet.service';
import { LoadingSpinnerComponent } from '../../../shared/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-lost-pets-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule,
    LoadingSpinnerComponent,
  ],
  templateUrl: './lost-pets-detail.component.html',
  styleUrl: './lost-pets-detail.component.css',
})
export class LostPetsDetailComponent {
  private route = inject(ActivatedRoute);
  public router = inject(Router);
  private lostPetService = inject(LostPetService);
  public translate = inject(TranslateService);
  private title = inject(Title);
  private meta = inject(Meta);
  //private authService = inject(AuthService);

  slug = toSignal(
    this.route.paramMap.pipe(
      switchMap(params => [params.get('slug')]),
      filter((slug): slug is string => slug !== null && slug !== undefined)
    )
  );
  lostPet = signal<LostPet | undefined>(undefined);
  //public isAuthenticated: Signal<boolean> = this.authService.isLoggedIn;
  // user: Signal<User | null> = signal(this.authService.currentUser());

  constructor() {
    // Ефект завантаження тварини за slug
    effect(() => {
      const slugValue = this.slug();
      if (!slugValue) return;

      this.lostPetService.getLostPetBySlug(slugValue).subscribe(lostPet => {
        if (!lostPet) {
          this.router.navigate(['/not-found']);
          return;
        }

        this.lostPet.set(lostPet);
        const translatedName = this.translate.instant('lostPet.name', {
          value: lostPet.name,
        });
        const translatedDescription = this.translate.instant(
          'lostPet.description',
          {
            value: lostPet.description,
          }
        );

        console.log('this.lostPet', this.lostPet());

        this.title.setTitle(`${translatedName} - PetCare`);
        this.meta.updateTag({
          name: 'description',
          content: translatedDescription || '',
        });
      });
    });
  }
}
