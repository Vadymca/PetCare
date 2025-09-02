import { CommonModule, isPlatformBrowser } from '@angular/common';
import {
  AfterViewInit,
  Component,
  effect,
  ElementRef,
  inject,
  PLATFORM_ID,
  QueryList,
  signal,
  ViewChildren,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from '../../core/services/auth.service';
import { ModalService } from '../../core/services/modal.service';
import { animateCounter } from '../../shared/animation/counter-animation';
import { AnimalsPreviewComponent } from '../../shared/components/animals-preview/animals-preview.component';
import { PrimaryLargeButtonComponent } from '../../shared/components/buttons/blue/primary-large-button.component';
import { SecondaryLargeButtonComponent } from '../../shared/components/buttons/blue/secondary-large-button.component';
import { SecondarySmallButtonComponent } from '../../shared/components/buttons/blue/secondary-small-button.component';
import { DownloadOrangeButtonWithIconComponent } from '../../shared/components/buttons/download-orange-button-with-icon.component';
import { PrimaryLargeOrangeButtonComponent } from '../../shared/components/buttons/orange/primary-large-orange-button.component';
import { FinancialSupportComponent } from '../../shared/components/financial-support/financial-support.component';
import { IconComponent } from '../../shared/components/icon.component';
import { HomeProjectsComponent } from "../../shared/components/home-projects/home-projects.component";
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    IconComponent,
    PrimaryLargeButtonComponent,
    SecondarySmallButtonComponent,
    PrimaryLargeOrangeButtonComponent,
    SecondaryLargeButtonComponent,
    DownloadOrangeButtonWithIconComponent,
    FinancialSupportComponent,
    AnimalsPreviewComponent,
    HomeProjectsComponent
],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements AfterViewInit {
  onTempClick() {
    this.router.navigate(['/not-found.component']);
  }
  @ViewChildren('counter') counters!: QueryList<ElementRef>;
  platformId = inject(PLATFORM_ID);
  values = [12000, 14067, 10068];

  isPlatformBrowser = isPlatformBrowser;

  ngAfterViewInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.counters.forEach((counter: ElementRef, index: number) => {
        animateCounter(counter.nativeElement, this.values[index], 2000);
      });
    }
  }
  onDownloadMonthlyReportClick() {
    throw new Error('Method not implemented.');
  }
  onAllReportsClick() {
    throw new Error('Method not implemented.');
  }
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private authService = inject(AuthService);
  private modalService = inject(ModalService);
  workers = [
    {
      id: 0,
      name: 'director',
      image: '../../../assets/images/director.png',
      title: 'DIRECTOR',
      firstDiv: 'FIRST_DIV_DIRECTOR',
      selectedSpan: 'SELECTED_SPAN_DIRECTOR',
      secondDiv: 'SECOND_DIV_DIRECTOR',
      thirdDiv: 'THIRD_DIV_DIRECTOR',
    },
    {
      id: 1,
      name: 'volunteer',
      image: '../../../assets/images/volunteer.png',
      title: 'VOLUNTEER',
      firstDiv: 'FIRST_DIV_VOLUNTEER',
      selectedSpan: 'SELECTED_SPAN_VOLUNTEER',
      secondDiv: 'SECOND_DIV_VOLUNTEER',
      thirdDiv: 'THIRD_DIV_VOLUNTEER',
    },
    {
      id: 2,
      name: 'vet',
      image: '../../../assets/images/vet.png',
      title: 'VET',
      firstDiv: 'FIRST_DIV_VET',
      selectedSpan: 'SELECTED_SPAN_VET',
      secondDiv: 'SECOND_DIV_VET',
      thirdDiv: 'THIRD_DIV_VET',
    },
  ];
  isImageChanging = false;
  currentWorkerIndex = 0;
  selectedWorker() {
    return this.workers[this.currentWorkerIndex];
  }
  isCharityButtonHidden = signal<boolean>(false);
  selectWorker(index: number) {
    this.isImageChanging = true; // робимо fade-out
    this.currentWorkerIndex = index; // міняємо картинку після fade-out
    setTimeout(() => {
      this.isImageChanging = false; // запускаємо fade-in
    }, 300); // 300мс = половина твоєї transition-duration
  }
  constructor() {
    effect(() => {
      if (this.modalService.modalStateReadonly().isOpen) {
        this.isCharityButtonHidden.set(true);
      } else {
        this.isCharityButtonHidden.set(false);
      }
    });
    this.route.queryParams.subscribe(params => {
      const token = params['token'];
      const currentPath = this.route.snapshot.routeConfig?.path;

      if (token && currentPath === 'verify-email') {
        this.authService.verifyEmail(token).subscribe({
          next: response => {
            if (response.success) {
              this.modalService.openModal('email-confirmed');
            } else {
              this.modalService.openModal('email-not-confirmed');
            }
            this.router.navigate([''], { queryParams: {}, replaceUrl: true });
          },
          error: err => {
            console.error('Verify email error:', err);
            this.modalService.openModal('email-not-confirmed');
            this.router.navigate([''], { queryParams: {}, replaceUrl: true });
          },
        });
      } else if (token && currentPath === 'reset-password') {
        this.modalService.setToken(token);
        this.modalService.openModal('reset-password');
        this.router.navigate([''], { queryParams: {}, replaceUrl: true });
      }
    });
  }
  onFindPetClick() {
    this.router.navigate(['/animals']);
  }
  onCharityButtonClick() {
    this.modalService.openModal('live-donation-collection');
  }
  onAboutUsClick() {
    throw new Error('Method not implemented.');
  }
}
