import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
): boolean | UrlTree => {
  const authService = inject(AuthService);
  const router = inject(Router);
  console.log('AuthGuard check');
  const user = authService.currentUser();
  console.log('AuthGuard check, user:', user);
  if (!user) {
    return router.createUrlTree(['/login'], {
      queryParams: { returnUrl: state.url },
    });
  }

  const requiredRole = route.data['role'] as string | undefined;

  if (requiredRole && user.role !== requiredRole) {
    return router.createUrlTree(['/access-denied']);
  }

  return true;
};
