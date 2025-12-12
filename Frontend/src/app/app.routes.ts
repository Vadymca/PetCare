// import { Routes } from '@angular/router';
// import { HelloWorldComponent } from './hello-world/hello-world.component';

// export const routes: Routes = [
//   { path: 'hello/:show', component: HelloWorldComponent },
//   { path: '', redirectTo: '/hello/true', pathMatch: 'full' },
//   { path: '**', redirectTo: '/hello/true' }
// ];

import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { metaResolver } from './resolvers/meta.resolver';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/home/home.component').then(c => c.HomeComponent),
    resolve: { meta: metaResolver },
  },
  {
    path: 'verify-email',
    loadComponent: () =>
      import('./pages/home/home.component').then(c => c.HomeComponent),
    resolve: { meta: metaResolver },
    data: { title: 'Підтвердження email — Добродій🐱' },
  },
  {
    path: 'reset-password',
    loadComponent: () =>
      import('./pages/home/home.component').then(c => c.HomeComponent),
    resolve: { meta: metaResolver },
    data: { title: 'Скидання паролю — Добродій🐱' },
  },
  {
    path: 'support',
    loadComponent: () =>
      import('./pages/care/support/support.component').then(
        c => c.SupportComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Притулок Добродій🐱 — як Ви можете допомогти?',
      description:
        'Стати волонтером, усиновити тварину, підтримати фінансово, опікуватися на відстані, організувати подію-зустріч ❤️',
      image: '/assets/images/support/support-women.jpg',
    },
  },
  {
    path: 'support-volunteering-form',
    loadComponent: () =>
      import(
        './pages/care/support-volunteering-form/support-volunteering-form.component'
      ).then(c => c.SupportVolunteeringFormComponent),
    resolve: { meta: metaResolver },
    data: {
      title: 'Добродій🐱 — як стати волонтером?',
      description:
        'Стати волонтером, усиновити тварину, підтримати фінансово, опікуватися на відстані, організувати подію-зустріч ❤️',
      image: '/assets/images/support/support-women.jpg',
    },
  },
  {
    path: 'care-rules',
    loadComponent: () =>
      import('./pages/care/care-rules/care-rules.component').then(
        c => c.CareRulesComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Добродій🐱 — умови опіки над твариною',
      description: 'Ставай другом на відстані для хвостика з притулку. ❤️',
      image: '/assets/images/support/support-women.jpg',
    },
  },
  {
    path: 'adoption-rules',
    loadComponent: () =>
      import(
        './pages/adoption-group/adoption-rules/adoption-rules.component'
      ).then(c => c.AdoptionRulesComponent),
    resolve: { meta: metaResolver },
    data: {
      title: 'Добродій🐱 — умови усиновлення тварини',
      description:
        'Не шукай породу — шукай душу. Вона чекає на тебе у притулку ❤️',
      image: '/assets/images/support/support-women.jpg',
    },
  },
  {
    path: 'volunteer-application-confirmation',
    loadComponent: () =>
      import(
        './pages/care/volunteer-application-confirmation/volunteer-application-confirmation.component'
      ).then(c => c.VolunteerApplicationConfirmationComponent),
    resolve: { meta: metaResolver },
  },
  {
    path: 'animals',

    loadComponent: () =>
      import('./features/animals/animal-list/animal-list.component').then(
        c => c.AnimalListComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Тварини в притулку Добродій🐱',
      description:
        'Знайди свого справжнього друга, серед тих, хто найбільше потребує любові ❤️',
      image: '/assets/images/adoption/dog_with_girl_in_orange_jumper.png',
    },
  },
  {
    path: 'animals/:slug',

    loadComponent: () =>
      import('./features/animals/animal-detail/animal-detail.component').then(
        c => c.AnimalDetailComponent
      ),
    resolve: { meta: metaResolver },
  },

  {
    path: 'shelters/:slug',
    loadComponent: () =>
      import(
        './features/shelters/shelter-detail/shelter-detail.component'
      ).then(c => c.ShelterDetailComponent),
    resolve: { meta: metaResolver },
  },
  {
    path: 'shelters/:slug/animals',
    loadComponent: () =>
      import(
        './features/shelters/shelter-animals/shelter-animals.component'
      ).then(c => c.ShelterAnimalsComponent),
    resolve: { meta: metaResolver },
    data: {
      title: 'Тварини в притулку Добродій🐱',
      description:
        'Знайди свого справжнього друга, серед тих, хто найбільше потребує любові ❤️',
      image: '/assets/images/adoption/dog_with_girl_in_orange_jumper.png',
    },
  },

  // {
  //   path: 'articles',
  //   loadComponent: () =>
  //     import('./features/articles/article-list/article-list.component').then(
  //       c => c.ArticleListComponent
  //     ),
  //   resolve: { meta: metaResolver },
  //   data: {
  //     title: 'Добродій — щасливі історії❤️',
  //     description:
  //       'Тут живуть спогади про котиків і песиків, які знайшли свою родину, любов і турботу. Вони доводять: добро перемагає!',
  //     image: '/assets/images/adoption-rules/hugs.png',
  //   },
  // },
  // {
  //   path: 'articles/:slug',
  //   loadComponent: () =>
  //     import(
  //       './features/articles/article-detail/article-detail.component'
  //     ).then(c => c.ArticleDetailComponent),
  //   resolve: { meta: metaResolver },
  // },
  {
    path: 'success-stories',
    loadComponent: () =>
      import(
        './features/successStories/succes-story-list/success-story-list.component'
      ).then(c => c.SuccessStoryListComponent),
    resolve: { meta: metaResolver },
    data: {
      title: 'Добродій🐱 — щасливі історії❤️',
      description:
        'Тут живуть спогади про котиків і песиків, які знайшли свою родину, любов і турботу. Вони доводять: добро перемагає!',
      image: '/assets/images/adoption-rules/hugs.png',
    },
  },
  // {
  //   path: 'success-stories/:slug',
  //   loadComponent: () =>
  //     import(
  //       './features/successStories/succes-story-detail/succes-story-detail.component'
  //     ).then(c => c.SuccesStoryDetailComponent),
  //   resolve: { meta: metaResolver },
  // },
  {
    path: `adoption`,
    loadComponent: () =>
      import('./pages/adoption-group/adoption/adoption.component').then(
        c => c.AdoptionComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Добродій🐱 — притулок для бездомних тварин',
      description: 'Зазирни в їхні очі і відчуй, хто чекає саме на тебе',
      image:
        'https://i.pinimg.com/1200x/fa/5d/fb/fa5dfb37d1fb28991dd6a468508f5091.jpg',
    },
  },
  // {
  //   path: 'lost-pets/:slug',
  //   loadComponent: () =>
  //     import(
  //       './features/lost-pets/lost-pets-detail/lost-pets-detail.component'
  //     ).then(c => c.LostPetsDetailComponent),
  //   resolve: { meta: metaResolver },
  // },
  {
    path: 'animal-aid-requests',
    loadComponent: () =>
      import(
        './features/animal-aid-request/animal-aid-request-list/animal-aid-request-list.component'
      ).then(c => c.AnimalAidRequestListComponent),
    resolve: { meta: metaResolver },
    data: {
      title: 'Благодійні проекти притулку Добродій🐱',
      description: 'Обирай, як ти можеш допомогти сьогодні! ❤️',
      image: '/assets/images/news/news-learning.jpg',
    },
  },
  {
    path: 'animal-aid-requests/:id',
    loadComponent: () =>
      import(
        './features/animal-aid-request/animal-aid-request-detail/animal-aid-request-detail.component'
      ).then(c => c.AnimalAidRequestDetailComponent),
    resolve: { meta: metaResolver },
  },
  {
    path: 'profile',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/user-account/profile/profile.component').then(
        c => c.ProfileComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Добродій🐱 - кабінет користувача🪪',
      description: 'Ваша особиста сторінка на сайті притулку Добродій🐱',
      image: '/assets/images/support/support1_cat.png',
    },
  },
  {
    path: 'favorites',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/favorites/favorites.component').then(
        c => c.FavoritesComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Притулок Добродій - Ваші улюблені тварини🐱🐶🐰',
      description: 'Сторінка доступна авторизованим користувачам',
      image: '/assets/images/background1.png',
    },
  },
  {
    path: 'profile/edit',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/user-account/edit-user/edit-user.component').then(
        c => c.EditUserComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Добродій🐱 - кабінет користувача🪪',
      description: 'Ваша особиста сторінка на сайті притулку Добродій🐱',
      image: '/assets/images/support/support1_cat.png',
    },
  },
  {
    path: 'profile/security',
    canActivate: [authGuard],
    loadComponent: () =>
      import(
        './pages/user-account/security-settings/security-settings.component'
      ).then(c => c.SecuritySettingsComponent),
    resolve: { meta: metaResolver },
    data: {
      title: 'Добродій🐱 - кабінет користувача🪪',
      description: 'Ваша особиста сторінка на сайті притулку Добродій🐱',
      image: '/assets/images/support/support1_cat.png',
    },
  },
  {
    path: 'about',
    loadComponent: () =>
      import('./pages/about/about.component').then(c => c.AboutComponent),
    resolve: { meta: metaResolver },
  },
  {
    path: 'news',
    loadComponent: () =>
      import('./pages/news-group/news/news.component').then(
        c => c.NewsComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Новини притулку Добродій🐱',
      description: 'Залишайтеся в курсі останніх новин притулку Добродій🐱📢',
      image: '/assets/images/support/support1_cat.png',
    },
  },
  {
    path: 'latest-news',
    loadComponent: () =>
      import('./pages/news-group/latest-news/latest-news.component').then(
        c => c.LatestNewsComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Новини притулку Добродій🐱',
      description: 'Залишайтеся в курсі останніх новин притулку Добродій🐱📢',
      image: '/assets/images/support/support1_cat.png',
    },
  },
  {
    path: 'news/:id',

    loadComponent: () =>
      import('./pages/news-group/news-detail/news-detail.component').then(
        c => c.NewsDetailComponent
      ),
    resolve: { meta: metaResolver },
  },

  {
    path: 'terms-and-conditions',
    loadComponent: () =>
      import(
        './pages/terms-and-conditions/terms-and-conditions.component'
      ).then(m => m.TermsAndConditionsComponent),
    resolve: { meta: metaResolver },
    data: {
      title: 'Правила та умови сайту Добродій🐱',
      description: 'Кожна Ваша гривня іде на добру справу 💵❤️',
      image: '/assets/images/background1.png',
    },
  },
  {
    path: 'privacy-policy',
    loadComponent: () =>
      import('./pages/privacy-policy/privacy-policy.component').then(
        m => m.PrivacyPolicyComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Політика конфіденційності притулку Добродій🐱',
      description: 'Ваші дані в безпеці та нікому не передаються',
      image: '/assets/images/background1.png',
    },
  },

  {
    path: 'public-offer',
    loadComponent: () =>
      import('./pages/public-offer/public-offer.component').then(
        m => m.PublicOfferComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Публічна оферта притулку Добродій🐱',
      description:
        'Публічна оферта про надання благодійної пожертви. Кожна Ваша гривня іде на добру справу 💵❤️',
      image: '/assets/images/background1.png',
    },
  },
  {
    path: 'reports',
    loadComponent: () =>
      import('./pages/reports/reports.component').then(m => m.ReportsComponent),
    resolve: { meta: metaResolver },
    data: {
      title: 'Звіти про роботу притулку Добродій🐱',
      description: 'Кожна Ваша гривня іде на добру справу 💵❤️',
      image:
        'https://i.pinimg.com/736x/e0/6c/f4/e06cf4e71af7e197021c1c4d6a165b74.jpg',
    },
  },
  {
    path: 'contacts',
    loadComponent: () =>
      import('./pages/contacts/contacts.component').then(
        m => m.ContactsComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Контакти притулку Добродій🐱',
      description: 'Ми поруч 🗺️📍',
      image:
        'https://i.pinimg.com/1200x/84/b1/3b/84b13bc41234eb3eca0f0e85b54f089d.jpg',
    },
  },
  {
    path: 'feedback-form',
    loadComponent: () =>
      import('./pages/feedback-form/feedback-form.component').then(
        m => m.FeedbackFormComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Зворотній зв’язок притулку Добродій🐱',
      description: 'Ваші відгуки важливі для нас ❤️',
      image:
        'https://i.pinimg.com/736x/9e/5d/d2/9e5dd2d63e9db5da1ce519054a3a2fed.jpg',
    },
  },
  {
    path: 'payment', // /payment
    redirectTo: 'payment/amount',
    pathMatch: 'full',
    resolve: { meta: metaResolver },
  },
  {
    path: 'payment/amount',
    loadComponent: () =>
      import('./pages/payment/payment-amount/payment-amount.component').then(
        m => m.PaymentAmountComponent
      ),
    resolve: { meta: metaResolver },
  },
  {
    path: 'payment/details',
    loadComponent: () =>
      import('./pages/payment/payment-details/payment-details.component').then(
        m => m.PaymentDetailsComponent
      ),
    resolve: { meta: metaResolver },
  },
  {
    path: 'payment-status',
    loadComponent: () =>
      import('./pages/payment/payment-status/payment-status.component').then(
        m => m.PaymentStatusComponent
      ),
    resolve: { meta: metaResolver },
  },
  {
    path: 'my-payments',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/payment/my-payments/my-payments.component').then(
        m => m.MyPaymentsComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Ваші благодійні платежі притулку Добродій🐱',
      description: 'Для хвостиків важлива кожна гривня ❤️',
      image:
        'https://i.pinimg.com/736x/46/90/20/46902093d28785e7c280dd86efadde33.jpg',
    },
  },

  {
    path: 'guardianships',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/guardianships/guardianships.component').then(
        m => m.GuardianshipsComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Ваші підопічні у притулку Добродій🐱',
      description: 'Дякуємо за небайдужість ❤️',
      image:
        'https://i.pinimg.com/736x/46/90/20/46902093d28785e7c280dd86efadde33.jpg',
    },
  },
  {
    path: 'access-denied',
    loadComponent: () =>
      import('./pages/errors/acces-denied/acces-denied.component').then(
        c => c.AccesDeniedComponent
      ),
    resolve: { meta: metaResolver },
  },
  {
    path: 'internal-server-error',
    loadComponent: () =>
      import(
        './pages/errors/internal-server-error/internal-server-error.component'
      ).then(c => c.InternalServerErrorComponent),
    resolve: { meta: metaResolver },
  },
  {
    path: 'service-unavailable',
    loadComponent: () =>
      import(
        './pages/errors/service-unavailable/service-unavailable.component'
      ).then(c => c.ServiceUnavailableComponent),
    resolve: { meta: metaResolver },
  },
  {
    path: 'team',
    loadComponent: () =>
      import('./pages/team-dobrodii/team/team.component').then(
        c => c.TeamComponent
      ),
    resolve: { meta: metaResolver },
    data: {
      title: 'Команда розробників сайту Добродій🐱',
      description: 'Дякуємо за увагу та чекаємо пропозицій роботи❤️',
      image:
        'https://i.pinimg.com/736x/57/aa/cc/57aacc999b2456019ff5c0ed9267d4da.jpg',
    },
  },

  {
    path: '**',
    loadComponent: () =>
      import('./pages/errors/not-found/not-found.component').then(
        m => m.NotFoundComponent
      ),
    resolve: { meta: metaResolver },
  },
];
