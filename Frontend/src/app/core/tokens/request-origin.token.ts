import { isPlatformBrowser } from '@angular/common';
import { inject, InjectionToken, PLATFORM_ID } from '@angular/core';

export const REQUEST_ORIGIN = new InjectionToken<string>('REQUEST_ORIGIN', {
  providedIn: 'root',
  factory: () => {
    const platformId = inject(PLATFORM_ID);
    if (isPlatformBrowser(platformId)) {
      return window.location.origin; // для браузера
    }
    return ''; // дефолт для сервера, але сервер все одно оверрайдить
  },
});
