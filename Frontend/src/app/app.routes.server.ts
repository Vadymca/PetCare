import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
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

  { path: 'animals', renderMode: RenderMode.Prerender }, // список тварин
  { path: 'articles', renderMode: RenderMode.Prerender }, // список статей
  { path: 'success-stories', renderMode: RenderMode.Prerender }, // список історій
  { path: 'animal-aid-requests', renderMode: RenderMode.Prerender }, // список заявок на допомогу

  { path: 'adoption', renderMode: RenderMode.Prerender },

  { path: 'about', renderMode: RenderMode.Prerender },
  { path: 'news', renderMode: RenderMode.Prerender },
  { path: 'terms-and-conditions', renderMode: RenderMode.Prerender },
  { path: 'privacy-policy', renderMode: RenderMode.Prerender },
  { path: 'public-offer', renderMode: RenderMode.Prerender },
  { path: 'reports', renderMode: RenderMode.Prerender },
  { path: 'contacts', renderMode: RenderMode.Prerender },
  { path: 'feedback-form', renderMode: RenderMode.Prerender },
  { path: 'team', renderMode: RenderMode.Prerender },

  // Payment flow
  { path: 'payment/amount', renderMode: RenderMode.Prerender },
  { path: 'payment/details', renderMode: RenderMode.Prerender },
  { path: 'payment-status', renderMode: RenderMode.Prerender },

  // Errors
  { path: 'access-denied', renderMode: RenderMode.Prerender },
  { path: 'internal-server-error', renderMode: RenderMode.Prerender },
  { path: 'service-unavailable', renderMode: RenderMode.Prerender },
];
