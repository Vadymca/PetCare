import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn } from '@angular/router';
import { MetaSsrService } from '../core/services/meta-ssr.service';

export const metaResolver: ResolveFn<null> = (
  route: ActivatedRouteSnapshot
) => {
  const meta = inject(MetaSsrService);

  // Беремо дані з route.data (ти їх сам задаси в роутах нижче)
  const title =
    (route.data['title'] as string) ?? 'Добродій — притулок для тварин';
  const description =
    (route.data['description'] as string) ??
    'Допомагаємо бездомним тваринам знайти дім 🐶🐱🐰❤️🏠';
  const image =
    (route.data['image'] as string) ?? '/assets/images/background1.png';
  const url =
    'https://dobrodii.onrender.com' +
    route.url.map(segment => segment.path).join('/');

  meta.update(title, description, image, url);

  return null;
};
