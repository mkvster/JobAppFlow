import { HttpContextToken, HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, from, switchMap, throwError } from 'rxjs';

import { AuthStateService } from './auth-state.service';

export const SKIP_AUTH_INTERCEPTOR = new HttpContextToken<boolean>(() => false);

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authState = inject(AuthStateService);
  if (req.context.get(SKIP_AUTH_INTERCEPTOR)) {
    return next(req);
  }

  const accessToken = authState.accessToken();
  const authedRequest = req.clone({
    withCredentials: true,
    setHeaders: accessToken ? { Authorization: `Bearer ${accessToken}` } : undefined
  });

  return next(authedRequest).pipe(
    catchError((error: unknown) => {
      if (!(error instanceof HttpErrorResponse) || error.status !== 401) {
        return throwError(() => error);
      }

      if (authState.isLoggingOut()) {
        return throwError(() => error);
      }

      return from(authState.refreshSession()).pipe(
        switchMap((session) => {
          if (!session?.accessToken) {
            return throwError(() => error);
          }

          return next(
            req.clone({
              withCredentials: true,
              setHeaders: { Authorization: `Bearer ${session.accessToken}` }
            })
          );
        }),
        catchError(() => throwError(() => error))
      );
    })
  );
};
