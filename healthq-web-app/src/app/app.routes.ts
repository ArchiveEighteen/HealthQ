import { Routes } from '@angular/router';
import { RegisterComponent } from './core/auth/pages/register/register.component';
import { LoginComponent } from './core/auth/pages/login/login.component';
import { QConstructorComponent } from './features/questionnaire/pages/q-constructor/q-constructor.component';
import { QuestionnaireComponent } from './features/questionnaire/pages/questionnaire/questionnaire.component';
import { authGuard } from './core/auth/auth.guard';
import { DMainPageComponent } from './features/actors/doctor/pages/d-main-page/d-main-page.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/doctor',
    pathMatch: 'full',
  },
  {
    path: 'doctor',
    component: DMainPageComponent,
    canActivate: [authGuard],
    title: 'Doctor Main Page',
  },
  {
    path: 'constructor',
    component: QConstructorComponent,
    canActivate: [authGuard],
    title: 'Constructor',
  },
  {
    path: 'questionnaire',
    component: QuestionnaireComponent,
    title: 'Questionnaire',
  },
  {
    path: 'login',
    component: LoginComponent,
    title: 'Login',
  },
  {
    path: 'signup',
    component: RegisterComponent,
    title: 'Register',
  },
];
