// import { Routes } from '@angular/router';
// import { HelloWorldComponent } from './hello-world/hello-world.component';

// export const routes: Routes = [
//   { path: 'hello/:show', component: HelloWorldComponent },
//   { path: '', redirectTo: '/hello/true', pathMatch: 'full' },
//   { path: '**', redirectTo: '/hello/true' }
// ];

import { Routes } from '@angular/router';
import { RenderMode } from '@angular/ssr';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/home/home.component').then(c => c.HomeComponent),
  },
  {
    path: 'verify-email',
    loadComponent: () =>
      import('./pages/home/home.component').then(c => c.HomeComponent),
  },
  {
    path: 'reset-password',
    loadComponent: () =>
      import('./pages/home/home.component').then(c => c.HomeComponent),
  },
  {
    path: 'support',
    loadComponent: () =>
      import('./pages/care/support/support.component').then(
        c => c.SupportComponent
      ),
  },
  {
    path: 'support-volunteering-form',
    loadComponent: () =>
      import(
        './pages/care/support-volunteering-form/support-volunteering-form.component'
      ).then(c => c.SupportVolunteeringFormComponent),
  },
  {
    path: 'care-rules',
    loadComponent: () =>
      import('./pages/care/care-rules/care-rules.component').then(
        c => c.CareRulesComponent
      ),
  },
  {
    path: 'adoption-rules',
    loadComponent: () =>
      import(
        './pages/adoption-group/adoption-rules/adoption-rules.component'
      ).then(c => c.AdoptionRulesComponent),
  },
  {
    path: 'volunteer-application-confirmation',
    loadComponent: () =>
      import(
        './pages/care/volunteer-application-confirmation/volunteer-application-confirmation.component'
      ).then(c => c.VolunteerApplicationConfirmationComponent),
  },
  {
    path: 'animals',

    loadComponent: () =>
      import('./features/animals/animal-list/animal-list.component').then(
        c => c.AnimalListComponent
      ),
  },
  {
    path: 'animals/:slug',

    loadComponent: () =>
      import('./features/animals/animal-detail/animal-detail.component').then(
        c => c.AnimalDetailComponent
      ),
    data: { renderMode: RenderMode.Server },
  },

  {
    path: 'shelters/:slug',
    loadComponent: () =>
      import(
        './features/shelters/shelter-detail/shelter-detail.component'
      ).then(c => c.ShelterDetailComponent),
  },
  {
    path: 'shelters/:slug/animals',
    loadComponent: () =>
      import(
        './features/shelters/shelter-animals/shelter-animals.component'
      ).then(c => c.ShelterAnimalsComponent),
    data: { renderMode: RenderMode.Server },
  },

  {
    path: 'articles',
    loadComponent: () =>
      import('./features/articles/article-list/article-list.component').then(
        c => c.ArticleListComponent
      ),
  },
  {
    path: 'articles/:slug',
    loadComponent: () =>
      import(
        './features/articles/article-detail/article-detail.component'
      ).then(c => c.ArticleDetailComponent),
    data: { renderMode: RenderMode.Server },
  },
  {
    path: 'success-stories',
    loadComponent: () =>
      import(
        './features/successStories/succes-story-list/success-story-list.component'
      ).then(c => c.SuccessStoryListComponent),
  },
  {
    path: 'success-stories/:slug',
    loadComponent: () =>
      import(
        './features/successStories/succes-story-detail/succes-story-detail.component'
      ).then(c => c.SuccesStoryDetailComponent),
    data: { renderMode: RenderMode.Server },
  },
  {
    path: `adoption`,
    loadComponent: () =>
      import('./pages/adoption-group/adoption/adoption.component').then(
        c => c.AdoptionComponent
      ),
  },
  // {
  //   path: 'lost-pets',
  //   loadComponent: () =>
  //     import(
  //       './features/lost-pets/lost-pets-list/lost-pets-list.component'
  //     ).then(c => c.LostPetsListComponent),
  // },
  {
    path: 'lost-pets/:slug',
    loadComponent: () =>
      import(
        './features/lost-pets/lost-pets-detail/lost-pets-detail.component'
      ).then(c => c.LostPetsDetailComponent),
    data: { renderMode: RenderMode.Server },
  },
  {
    path: 'animal-aid-requests',
    loadComponent: () =>
      import(
        './features/animal-aid-request/animal-aid-request-list/animal-aid-request-list.component'
      ).then(c => c.AnimalAidRequestListComponent),
  },
  {
    path: 'animal-aid-requests/:id',
    loadComponent: () =>
      import(
        './features/animal-aid-request/animal-aid-request-detail/animal-aid-request-detail.component'
      ).then(c => c.AnimalAidRequestDetailComponent),
    data: { renderMode: RenderMode.Server },
  },
  {
    path: 'profile',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/user-account/profile/profile.component').then(
        c => c.ProfileComponent
      ),
  },
  {
    path: 'favorites',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/favorites/favorites.component').then(
        c => c.FavoritesComponent
      ),
  },
  {
    path: 'profile/edit',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/user-account/edit-user/edit-user.component').then(
        c => c.EditUserComponent
      ),
  },
  {
    path: 'profile/security',
    canActivate: [authGuard],
    loadComponent: () =>
      import(
        './pages/user-account/security-settings/security-settings.component'
      ).then(c => c.SecuritySettingsComponent),
  },
  {
    path: 'about',
    loadComponent: () =>
      import('./pages/about/about.component').then(c => c.AboutComponent),
  },
  {
    path: 'news',
    loadComponent: () =>
      import('./pages/news-group/news/news.component').then(
        c => c.NewsComponent
      ),
  },

  {
    path: 'terms-and-conditions',
    loadComponent: () =>
      import(
        './pages/terms-and-conditions/terms-and-conditions.component'
      ).then(m => m.TermsAndConditionsComponent),
  },
  {
    path: 'privacy-policy',
    loadComponent: () =>
      import('./pages/privacy-policy/privacy-policy.component').then(
        m => m.PrivacyPolicyComponent
      ),
  },

  {
    path: 'public-offer',
    loadComponent: () =>
      import('./pages/public-offer/public-offer.component').then(
        m => m.PublicOfferComponent
      ),
  },
  {
    path: 'reports',
    loadComponent: () =>
      import('./pages/reports/reports.component').then(m => m.ReportsComponent),
  },
  {
    path: 'contacts',
    loadComponent: () =>
      import('./pages/contacts/contacts.component').then(
        m => m.ContactsComponent
      ),
  },
  {
    path: 'feedback-form',
    loadComponent: () =>
      import('./pages/feedback-form/feedback-form.component').then(
        m => m.FeedbackFormComponent
      ),
  },
  {
    path: 'payment', // /payment
    redirectTo: 'payment/amount',
    pathMatch: 'full',
  },
  {
    path: 'payment/amount',
    loadComponent: () =>
      import('./pages/payment/payment-amount/payment-amount.component').then(
        m => m.PaymentAmountComponent
      ),
  },
  {
    path: 'payment/details',
    loadComponent: () =>
      import('./pages/payment/payment-details/payment-details.component').then(
        m => m.PaymentDetailsComponent
      ),
  },
  {
    path: 'payment-status',
    loadComponent: () =>
      import('./pages/payment/payment-status/payment-status.component').then(
        m => m.PaymentStatusComponent
      ),
  },
  {
    path: 'my-payments',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/payment/my-payments/my-payments.component').then(
        m => m.MyPaymentsComponent
      ),
  },

  {
    path: 'guardianships',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/guardianships/guardianships.component').then(
        m => m.GuardianshipsComponent
      ),
  },
  {
    path: 'access-denied',
    loadComponent: () =>
      import('./pages/errors/acces-denied/acces-denied.component').then(
        c => c.AccesDeniedComponent
      ),
  },
  {
    path: 'internal-server-error',
    loadComponent: () =>
      import(
        './pages/errors/internal-server-error/internal-server-error.component'
      ).then(c => c.InternalServerErrorComponent),
  },
  {
    path: 'service-unavailable',
    loadComponent: () =>
      import(
        './pages/errors/service-unavailable/service-unavailable.component'
      ).then(c => c.ServiceUnavailableComponent),
  },
  {
    path: 'team',
    loadComponent: () =>
      import('./pages/team-dobrodii/team/team.component').then(
        c => c.TeamComponent
      ),
  },

  {
    path: '**',
    loadComponent: () =>
      import('./pages/errors/not-found/not-found.component').then(
        m => m.NotFoundComponent
      ),
  },
];
