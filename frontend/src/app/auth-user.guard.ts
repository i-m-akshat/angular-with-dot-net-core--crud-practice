import { inject } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';

export const authUserGuard: CanActivateFn = (
  route,
  state
): boolean | UrlTree => {
  const router = inject(Router);
  const token = localStorage.getItem('jwt');
  if (token) {
    return true;
  } else {
    return router.parseUrl('/user/login');
  }
};
