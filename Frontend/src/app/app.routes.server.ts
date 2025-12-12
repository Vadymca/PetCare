import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  // 1. Статичні маршрути — прендеримо (швидко + SEO)
  { path: '', renderMode: RenderMode.Prerender },
  { path: 'verify-email', renderMode: RenderMode.Prerender },
  { path: 'reset-password', renderMode: RenderMode.Prerender },
  { path: 'support', renderMode: RenderMode.Prerender },
  { path: 'support-volunteering-form', renderMode: RenderMode.Prerender },
  { path: 'care-rules', renderMode: RenderMode.Prerender },
  { path: 'adoption-rules', renderMode: RenderMode.Prerender },
  {
    path: 'volunteer-application-confirmation',
    renderMode: RenderMode.Prerender,
  },
  { path: 'animals', renderMode: RenderMode.Prerender },
  { path: 'success-stories', renderMode: RenderMode.Prerender },
  { path: 'animal-aid-requests', renderMode: RenderMode.Prerender },
  { path: 'adoption', renderMode: RenderMode.Prerender },
  { path: 'about', renderMode: RenderMode.Prerender },
  { path: 'news', renderMode: RenderMode.Prerender },
  { path: 'terms-and-conditions', renderMode: RenderMode.Prerender },
  { path: 'privacy-policy', renderMode: RenderMode.Prerender },
  { path: 'public-offer', renderMode: RenderMode.Prerender },
  { path: 'reports', renderMode: RenderMode.Prerender },
  { path: 'contacts', renderMode: RenderMode.Prerender },
  { path: 'feedback-form', renderMode: RenderMode.Prerender },
  { path: 'payment/amount', renderMode: RenderMode.Prerender },
  { path: 'payment/details', renderMode: RenderMode.Prerender },
  { path: 'payment-status', renderMode: RenderMode.Prerender },
  { path: 'team', renderMode: RenderMode.Prerender },
  { path: 'access-denied', renderMode: RenderMode.Prerender },
  { path: 'internal-server-error', renderMode: RenderMode.Prerender },
  { path: 'service-unavailable', renderMode: RenderMode.Prerender },

  // 2. УСІ динамічні + авторизовані + решта — SSR (обов’язково вказати!)
  { path: 'animals/:slug', renderMode: RenderMode.Server },
  { path: 'shelters/:slug', renderMode: RenderMode.Server },
  { path: 'shelters/:slug/animals', renderMode: RenderMode.Server },
  { path: 'animal-aid-requests/:id', renderMode: RenderMode.Server },
  { path: 'news/:id', renderMode: RenderMode.Prerender },

  { path: 'profile', renderMode: RenderMode.Server },
  { path: 'profile/edit', renderMode: RenderMode.Server },
  { path: 'profile/security', renderMode: RenderMode.Server },
  { path: 'favorites', renderMode: RenderMode.Server },
  { path: 'my-payments', renderMode: RenderMode.Server },
  { path: 'guardianships', renderMode: RenderMode.Server },

  // payment redirect — теж треба вказати
  { path: 'payment', renderMode: RenderMode.Server },

  // 3. 404 та будь-які інші (обов’язково в кінці!)
  { path: '**', renderMode: RenderMode.Server },
];
