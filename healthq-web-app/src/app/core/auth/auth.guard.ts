import { inject } from '@angular/core';
import {AuthService} from './auth.service';
import {Router} from '@angular/router';

export const authGuard = async () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const isAuthenticated = await authService.checkAuthenticated();
  console.log("Authentication check: ", isAuthenticated);

  if(isAuthenticated) {
    authService.retrieveUsername();
    return true;
  }
  return router.parseUrl('/login');
};
