import { Routes } from '@angular/router';
import { RegisterComponent } from './core/auth/pages/register/register.component';
import { LoginComponent } from './core/auth/pages/login/login.component';
import { QConstructorComponent } from './features/questionnaire/pages/q-constructor/q-constructor.component';
import { QuestionnaireComponent } from './features/questionnaire/pages/questionnaire/questionnaire.component';
import { authGuard } from './core/auth/auth.guard';
import { DMainPageComponent } from './features/actors/doctor/pages/d-main-page/d-main-page.component';
import { PMainPageComponent } from './features/actors/patient/pages/p-main-page/p-main-page.component';
import { patientGuard } from './features/actors/patient/patient.guard';
import { RedirectComponent } from './core/auth/redirect/redirect.component';
import { DTemplatesPageComponent } from './features/actors/doctor/pages/d-templates-page/d-templates-page.component';
import { DPatientsPageComponent } from './features/actors/doctor/pages/d-patients-page/d-patients-page.component';
import {
  PQuestionnairesPageComponent
} from './features/actors/patient/pages/p-questionnaires-page/p-questionnaires-page.component';
import {PDoctorsPageComponent} from './features/actors/patient/pages/p-doctors-page/p-doctors-page.component';
import {ProfileComponent} from './core/auth/pages/profile/profile.component';
import {AMainPageComponent} from './features/actors/administrator/pages/a-main-page/a-main-page.component';
import {ADoctorsPageComponent} from './features/actors/administrator/pages/a-doctors-page/a-doctors-page.component';
import {
  ADoctorProfilePageComponent
} from './features/actors/administrator/pages/a-doctor-profile-page/a-doctor-profile-page.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'redirect',
    pathMatch: 'full',
  },
  {
    path: 'redirect',
    component: RedirectComponent,
    title: 'Redirect Page',
  },
  {
    path: 'Doctor',
    component: DMainPageComponent,
    canActivate: [authGuard],
    title: 'Doctor Main Page',
    children: [
      {
        path: '',
        redirectTo: 'patients',
        pathMatch: 'full',
      },
      {
        path: 'templates',
        component: DTemplatesPageComponent,
        canActivate: [authGuard],
        title: 'Doctor Templates Page',
      },
      {
        path: 'patients',
        component: DPatientsPageComponent,
        canActivate: [authGuard],
        title: 'Doctor Patients Page',
      },
      {
        path: 'constructor',
        component: QConstructorComponent,
        canActivate: [authGuard],
        title: 'Constructor',
      },
    ],
  },
  {
    path: 'Patient',
    component: PMainPageComponent,
    canActivate: [authGuard, patientGuard],
    title: 'Patient Main Page',
    children: [
      {
        path: 'questionnaires',
        component: PQuestionnairesPageComponent,
        canActivate: [authGuard],
        title: 'My Questionnaires',
      },
      {
        path: 'doctors',
        component: PDoctorsPageComponent,
        canActivate: [authGuard],
        title: 'My Doctors',
      },
    ]
  },
  {
    path: 'Administrator',
    component: AMainPageComponent,
    canActivate: [authGuard],
    title: 'Administrator Main Page',
    children: [
      {
        path: 'doctors',
        component: ADoctorsPageComponent,
        canActivate: [authGuard],
        title: 'Doctors',
      },
      {
        path: 'doctor/:email',
        component: ADoctorProfilePageComponent,
        canActivate: [authGuard],
        title: 'Doctor',
      },
    ]
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
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [authGuard],
    title: 'Profile',
  }
];
