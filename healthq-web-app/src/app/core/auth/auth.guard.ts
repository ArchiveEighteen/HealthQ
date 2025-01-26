import { inject } from '@angular/core';
import {AuthService} from './auth.service';
import {Router} from '@angular/router';
import {map, subscribeOn, Subscription, take, tap} from 'rxjs';

export const authGuard = async () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const isAuthenticated = await authService.checkAuthenticated();
  console.log("Authentication check: ", isAuthenticated);

  return isAuthenticated ? true : router.parseUrl('/login');
};
