// import { Routes } from '@angular/router';
// import { HelloWorldComponent } from './hello-world/hello-world.component';

// export const routes: Routes = [
//   { path: 'hello/:show', component: HelloWorldComponent },
//   { path: '', redirectTo: '/hello/true', pathMatch: 'full' },
//   { path: '**', redirectTo: '/hello/true' }
// ];

import { Routes } from '@angular/router';

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
    path: '',
    redirectTo: 'animals',
    pathMatch: 'full',
  },
  {
    path: '**',
    redirectTo: 'animals',
  },
];
