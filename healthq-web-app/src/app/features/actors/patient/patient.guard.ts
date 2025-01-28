import { CanActivateFn } from '@angular/router';

export const patientGuard: CanActivateFn = (route, state) => {
  return true;
};
