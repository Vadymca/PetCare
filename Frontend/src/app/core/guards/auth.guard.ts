import { isPlatformBrowser } from '@angular/common';
import { inject, PLATFORM_ID } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { ModalService } from '../services/modal.service';

export const authGuard: CanActivateFn = (route, state) => {
  const platformId = inject(PLATFORM_ID);
  const authService = inject(AuthService);
  const router = inject(Router);
  const modalService = inject(ModalService);

  if (!isPlatformBrowser(platformId)) {
    return true;
  }

  return authService.refreshToken().pipe(
    // <-- async refresh
    map(data => {
      if (!data) {
        authService.setReturnUrl(state.url);
        modalService.openModal('welcome');
        return false;
      }

      const requiredRole = route.data['role'] as string | undefined;
      if (requiredRole && data.user?.role !== requiredRole) {
        return router.createUrlTree(['/access-denied']);
      }

      return true;
    }),
    catchError(
      () => {
        authService.setReturnUrl(state.url);
        modalService.openModal('welcome');
        return of(false);
      }
      // of(
      //   router.createUrlTree(['/'], {
      //     queryParams: { returnUrl: state.url },
      //   })
      // )
    )
  );
};
