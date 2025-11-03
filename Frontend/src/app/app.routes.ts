// import { Routes } from '@angular/router';
// import { HelloWorldComponent } from './hello-world/hello-world.component';

// export const routes: Routes = [
//   { path: 'hello/:show', component: HelloWorldComponent },
//   { path: '', redirectTo: '/hello/true', pathMatch: 'full' },
//   { path: '**', redirectTo: '/hello/true' }
// ];

import { Routes } from '@angular/router';
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
      ).then(
        c => c.AdoptionRulesComponent // c.AdoptionRul
      ),
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
  },

  {
    path: 'shelters/:slug',
    loadComponent: () =>
      import(
        './features/shelters/shelter-detail/shelter-detail.component'
      ).then(c => c.ShelterDetailComponent),
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
  },
  {
    path: `adoption`,
    loadComponent: () =>
      import('./pages/adoption-group/adoption/adoption.component').then(
        c => c.AdoptionComponent
      ),
  },
  {
    path: 'lost-pets',
    loadComponent: () =>
      import(
        './features/lost-pets/lost-pets-list/lost-pets-list.component'
      ).then(c => c.LostPetsListComponent),
  },
  {
    path: 'lost-pets/:slug',
    loadComponent: () =>
      import(
        './features/lost-pets/lost-pets-detail/lost-pets-detail.component'
      ).then(c => c.LostPetsDetailComponent),
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
  },

  // {
  //   path: 'user-profile',
  //   canActivate: [authGuard],
  //   loadComponent: () =>
  //     import('./pages/user-account/user-profile/user-profile.component').then(
  //       c => c.UserProfileComponent
  //     ),

  //   data: { roles: ['Admin'] }, // хто може, крім власника
  // },
  {
    path: 'profile',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/user-account/profile/profile.component').then(
        c => c.ProfileComponent
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
      import('./pages/team-dobrodii/team/team.component').then(c => c.TeamComponent),
  },

  {
    path: '**',
    loadComponent: () =>
      import('./pages/errors/not-found/not-found.component').then(
        m => m.NotFoundComponent
      ),
  },
];
