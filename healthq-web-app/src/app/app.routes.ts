import { Routes } from '@angular/router';
import {RegisterComponent} from './core/auth/pages/register/register.component';
import {LoginComponent} from './core/auth/pages/login/login.component';
import {QConstructorComponent} from './features/q-constructor/pages/q-constructor/q-constructor.component';
import {authGuard} from './core/auth/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/constructor',
    pathMatch: 'full',
  },
  {
    path: 'constructor',
    component: QConstructorComponent,
    canActivate: [authGuard],
    title: 'Constructor'
  },
  {
    path: 'login',
    component: LoginComponent,
    title: 'Login'
  },
  {
    path: 'signup',
    component: RegisterComponent,
    title: 'Register'
  }
];
