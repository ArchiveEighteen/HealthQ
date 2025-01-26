import { inject } from '@angular/core';
import {AuthService} from './auth.service';
import {Router} from '@angular/router';
import {subscribeOn, Subscription, tap} from 'rxjs';

export const authGuard = () => {
  const authService = inject(AuthService);
  const router = inject(Router);


  if(authService.$isLoggedIn) {
    console.log('authService: ', authService.$isLoggedIn);
    return true;
  }
  return router.parseUrl('/login');
};
