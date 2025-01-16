import { Routes } from '@angular/router';
import { QConstructorComponent } from './pages/q-constructor/q-constructor.component';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: QConstructorComponent,
  },
];
