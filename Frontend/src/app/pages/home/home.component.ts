import { CommonModule, isPlatformBrowser } from '@angular/common';
import {
  AfterViewInit,
  Component,
  effect,
  ElementRef,
  inject,
  OnDestroy,
  OnInit,
  PLATFORM_ID,
  QueryList,
  signal,
  ViewChildren,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { Subscription, take } from 'rxjs';
import { PaymentScope } from '../../core/models/liqPayCheckoutRequest';
import { AuthService } from '../../core/services/auth.service';
import { LiqPayService } from '../../core/services/liq-pay-service.service';
import { ModalService } from '../../core/services/modal.service';
import { animateCounter } from '../../shared/animation/counter-animation';
import { AnimalsPreviewComponent } from '../../shared/components/animals-preview/animals-preview.component';
import { PrimaryLargeButtonComponent } from '../../shared/components/buttons/blue/primary-large-button.component';
import { SecondaryLargeButtonComponent } from '../../shared/components/buttons/blue/secondary-large-button.component';
import { SecondarySmallButtonComponent } from '../../shared/components/buttons/blue/secondary-small-button.component';
import { DownloadOrangeButtonWithIconComponent } from '../../shared/components/buttons/orange/download-orange-button-with-icon.component';
import { PrimaryLargeOrangeButtonComponent } from '../../shared/components/buttons/orange/primary-large-orange-button.component';
import { FinancialSupportComponent } from '../../shared/components/financial-support/financial-support.component';
import { HomeNewsComponent } from '../../shared/components/home-news/home-news.component';
import { HomePartnersComponent } from '../../shared/components/home-partners/home-partners.component';
import { HomeProjectsComponent } from '../../shared/components/home-projects/home-projects.component';
import { IconComponent } from '../../shared/components/icon.component';
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
    HomeProjectsComponent,
    HomeNewsComponent,
    HomePartnersComponent,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements AfterViewInit, OnDestroy, OnInit {
  @ViewChildren('counter') counters!: QueryList<ElementRef>;
  platformId = inject(PLATFORM_ID);

  isPlatformBrowser = isPlatformBrowser;
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private authService = inject(AuthService);
  private modalService = inject(ModalService);
  private queryParamsSubscription?: Subscription;
  private isProcessed = false; // Флаг для запобігання повторної обробки
  isImageChanging = false;
  currentWorkerIndex = 0;
  values = [12000, 14067, 10068];
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
  reports = [
    { month: 1, year: 2024, link: 'тут буде посилання на звіт за січень 2024' },
    { month: 2, year: 2024, link: 'тут буде посилання на звіт за лютий 2024' },
    {
      month: 3,
      year: 2024,
      link: 'тут буде посилання на звіт за березень 2024',
    },
    {
      month: 4,
      year: 2024,
      link: 'тут буде посилання на звіт за квітень 2024',
    },
    {
      month: 5,
      year: 2024,
      link: 'тут буде посилання на звіт за травень 2024',
    },
    {
      month: 6,
      year: 2024,
      link: 'тут буде посилання на звіт за червень 2024',
    },
    { month: 7, year: 2024, link: 'тут буде посилання на звіт за липень 2024' },
    {
      month: 8,
      year: 2024,
      link: 'тут буде посилання на звіт за серпень 2024',
    },
    {
      month: 9,
      year: 2024,
      link: 'тут буде посилання на звіт за вересень 2024',
    },
    {
      month: 10,
      year: 2024,
      link: 'тут буде посилання на звіт за жовтень 2024',
    },
    {
      month: 11,
      year: 2024,
      link: 'тут буде посилання на звіт за листопад 2024',
    },
    {
      month: 12,
      year: 2024,
      link: 'тут буде посилання на звіт за грудень 2024',
    },
    { month: 1, year: 2025, link: 'тут буде посилання на звіт за січень 2025' },
    { month: 2, year: 2025, link: 'тут буде посилання на звіт за лютий 2025' },
    {
      month: 3,
      year: 2025,
      link: 'тут буде посилання на звіт за березень 2025',
    },
    {
      month: 4,
      year: 2025,
      link: 'тут буде посилання на звіт за квітень 2025',
    },
    {
      month: 5,
      year: 2025,
      link: 'тут буде посилання на звіт за травень 2025',
    },
    {
      month: 6,
      year: 2025,
      link: 'тут буде посилання на звіт за червень 2025',
    },
    { month: 7, year: 2025, link: 'тут буде посилання на звіт за липень 2025' },
    {
      month: 8,
      year: 2025,
      link: 'тут буде посилання на звіт за серпень 2025',
    },
  ];

  constructor() {
    effect(() => {
      if (this.modalService.modalStateReadonly().isOpen) {
        this.isCharityButtonHidden.set(true);
      } else {
        this.isCharityButtonHidden.set(false);
      }
      // if (isPlatformBrowser(this.platformId)) {
      //   this.router.events
      //     .pipe(filter(event => event instanceof NavigationEnd))
      //     .subscribe(() => {
      //       window.scrollTo({ top: 0, behavior: 'auto' });
      //     });
      // }
    });
  }
  ngOnInit() {
    // Обробка query-параметрів у ngOnInit
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    this.queryParamsSubscription = this.route.queryParams
      .pipe(take(1))
      .subscribe(params => {
        if (this.isProcessed) {
          return;
        }

        const token = params['token']
          ? decodeURIComponent(params['token'])
          : '';
        const email = params['email']
          ? decodeURIComponent(params['email']).trim()
          : '';
        const currentPath = this.route.snapshot.routeConfig?.path;

        // Перевірка валідності Base64
        // const isValidBase64 = token ? /^[A-Za-z0-9+/=]+$/.test(token) : false;
        // console.log('Is token valid Base64?', isValidBase64);

        if (email && token && currentPath === 'verify-email') {
          this.isProcessed = true; // Помічаємо, що запит оброблено
          this.authService.verifyEmail(email, token).subscribe({
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
        } else if (email && token && currentPath === 'reset-password') {
          this.isProcessed = true; // Помічаємо, що запит оброблено
          this.modalService.setTokenForResettingPassword(token);
          this.modalService.setEmailForResettingPassword(email);
          this.modalService.openModal('reset-password');
          this.router.navigate([''], { queryParams: {}, replaceUrl: true });
        }
      });
  }
  ngOnDestroy() {
    // Відписуємося від queryParams
    if (this.queryParamsSubscription) {
      this.queryParamsSubscription.unsubscribe();
    }
  }
  ngAfterViewInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.counters.forEach((counter: ElementRef, index: number) => {
        animateCounter(counter.nativeElement, this.values[index], 2000);
      });
    }
  }
  onSupportClick() {
    this.router.navigate(['/support']);
  }
  // onTempClick() {
  //   this.router.navigate(['/public-offer']);
  // }
  onDownloadMonthlyReportClick() {
    const month = new Date().getMonth();
    const year = new Date().getFullYear();
    let link = '';
    this.reports.forEach(report => {
      if (report.month === month && report.year === year) {
        link = report.link;
      }
    });
    window.open(link, '_blank');
  }
  onAllReportsClick() {
    this.router.navigate(['/reports']);
  }
  selectedWorker() {
    return this.workers[this.currentWorkerIndex];
  }
  isCharityButtonHidden = signal<boolean>(false);
  selectWorker(index: number) {
    this.isImageChanging = true; // робимо fade-out
    this.currentWorkerIndex = index; // міняємо картинку після fade-out
    setTimeout(() => {
      this.isImageChanging = false; // запускаємо fade-in
    }, 300);
  }
  onFindPetClick() {
    this.router.navigate(['/adoption']);
  }
  onCharityButtonClick() {
    this.modalService.openModal('live-donation-collection');
  }
  onAboutUsClick() {
    this.router.navigate(['/about']);
  }
  //по платежах
  private selectedAmount: number | null = null;
  private isRecurring = false;
  private liqPay = inject(LiqPayService);

  onSelectionConfirmed(selection: { amount: number; isOnce: boolean }) {
    this.selectedAmount = selection.amount;
    this.isRecurring = !selection.isOnce;
    if (this.selectedAmount !== null) {
      this.startGlobalPayment();
    } else {
      console.warn('Selected amount is null');
    }
  }

  private startGlobalPayment() {
    // Очищаємо старий контекст + записуємо новий глобальний
    this.liqPay.startPayment({
      scope: 'global' as PaymentScope,
      amount: this.selectedAmount!,
      isRecurring: this.isRecurring,
      description: this.isRecurring
        ? 'Щомісячна підтримка притулку'
        : 'Разова підтримка притулку',
    });

    // Переходимо до форми з контактами
    //поміняти потім
    this.router.navigate(['/payment/details']);
  }
}
