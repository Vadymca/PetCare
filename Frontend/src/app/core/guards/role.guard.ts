import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export function roleGuard(allowedRoles: string[]): CanActivateFn {
  return () => {
    const auth = inject(AuthService);
    const router = inject(Router);
    const user = auth.currentUser();
    const userRole = user?.role ?? 'User';

    if (allowedRoles.includes(userRole)) {
      return true;
    }

    // Якщо роль не підходить — редірект на "Доступ заборонено"
    return router.createUrlTree(['/access-denied']);
  };
}
