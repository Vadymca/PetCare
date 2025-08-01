import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const login2faGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  const step = auth.getAuthStep();

  if (step === '2fa') {
    return true;
  } else {
    router.navigate(['/login']);
    return false;
  }
};
