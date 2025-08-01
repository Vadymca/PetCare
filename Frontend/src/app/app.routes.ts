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
    canActivate: [authGuard],
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
