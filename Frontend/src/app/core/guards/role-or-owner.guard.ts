import { inject, Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class RoleOrOwnerGuard implements CanActivate {
  private authService = inject(AuthService);
  private router = inject(Router);

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean | UrlTree {
    const ownerId = route.paramMap.get('id');
    const allowedRoles: string[] = route.data['roles'] || [];

    const user = this.authService.currentUser();

    if (!user) {
      return this.router.createUrlTree(['/login'], {
        queryParams: { returnUrl: state.url },
      });
    }

    const isOwner = ownerId && user.id === ownerId;
    const hasRole = allowedRoles.includes(user.role);

    if (isOwner || hasRole) {
      return true;
    }

    return this.router.createUrlTree(['/access-denied']);
  }
}
