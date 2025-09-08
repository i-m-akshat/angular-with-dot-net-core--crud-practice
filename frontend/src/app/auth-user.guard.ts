import { inject } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';
import { TOKEN_KEY } from './shared/constants';

export const authUserGuard: CanActivateFn = (
  route,
  state
): boolean | UrlTree => {
  const router = inject(Router);
  const token = localStorage.getItem(TOKEN_KEY);
  console.log(token);
  if (token) {
    return true;
  } else {
    return router.parseUrl('/user/login');
  }
};
