import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { tap } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const toastr = inject(ToastrService);
  const authService = inject(AuthService);
  const isLoggedIn = authService.getLoginStatus_synchronous();
  if (isLoggedIn) {
    const clonedReq = req.clone({
      headers: req.headers.set(
        'Authorization',
        'Bearer ' + authService.getToken()
      ),
    });

    return next(clonedReq).pipe(
      tap({
        error: (err: any) => {
          if (err.status == 401) {
            authService.deleteToken();
            setTimeout(() => {
              toastr.info('Please login again', 'Session Expired !');
            }, 1500);
          }
        },
      })
    );
  } else {
    return next(req);
  }
};
