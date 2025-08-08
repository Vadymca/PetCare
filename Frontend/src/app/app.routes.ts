// import { Routes } from '@angular/router';
// import { HelloWorldComponent } from './hello-world/hello-world.component';

// export const routes: Routes = [
//   { path: 'hello/:show', component: HelloWorldComponent },
//   { path: '', redirectTo: '/hello/true', pathMatch: 'full' },
//   { path: '**', redirectTo: '/hello/true' }
// ];

import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { login2faGuard } from './core/guards/login-2fa.guard';

export const routes: Routes = [
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
    path: 'shelters',
    loadComponent: () =>
      import('./features/shelters/shelter-list/shelter-list.component').then(
        c => c.ShelterListComponent
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

  {
    path: 'profile',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/user-profile/user-profile.component').then(
        c => c.UserProfileComponent
      ),

    data: { roles: ['Admin'] }, // хто може, крім власника
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./pages/auth/login/login.component').then(c => c.LoginComponent),
  },
  {
    path: 'login-2fa',
    canActivate: [login2faGuard],
    loadComponent: () =>
      import('./pages/auth/login2fa/login2fa.component').then(
        c => c.Login2faComponent
      ),
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./pages/auth/register/register.component').then(
        m => m.RegisterComponent
      ),
  },
  {
    path: 'forgot-password',
    loadComponent: () =>
      import('./pages/auth/forgot-password/forgot-password.component').then(
        m => m.ForgotPasswordComponent
      ),
  },
  //перевірити потім, чи з бека так повертатиметься чи ?token=...
  {
    path: 'reset-password/:token',
    loadComponent: () =>
      import('./pages/auth/reset-password/reset-password.component').then(
        m => m.ResetPasswordComponent
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
    path: '',
    redirectTo: 'animals',
    pathMatch: 'full',
  },
  {
    path: '**',
    loadComponent: () =>
      import('./pages/errors/not-found/not-found.component').then(
        m => m.NotFoundComponent
      ),
  },
];
