import { inject } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';
import { TOKEN_KEY } from './shared/constants';
import { AuthService } from './shared/services/auth.service';

export const authUserGuard: CanActivateFn = (
  route,
  state
): boolean | UrlTree => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const token = localStorage.getItem(TOKEN_KEY);
  console.log(token);
  if (token) {
    const claimRequirement = route.data['claimReq'] as Function;
    if (claimRequirement) {
      const claims = authService.getClaims();
      if (claimRequirement(claims)) {
        return true;
      } else {
        return router.parseUrl('/forbidden');
      }
    } else {
      return true;
    }
  } else {
    return router.parseUrl('/user/login');
  }
};
