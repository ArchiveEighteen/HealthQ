import {Component, inject} from '@angular/core';
import {AuthService} from '../auth.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-redirect',
  imports: [],
  template: '',
  styles: ''
})
export class RedirectComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  private role: string | null;

  constructor() {
    this.role = this.authService.checkUserRole();

    if(this.role === null) {
      this.authService.getUserWithToken().then(role => {
        if(role != null) {
          console.log("Role was null but now dont");
          this.router.navigate(['/' + role]);
        }else{
          console.log("role was null and still null");
          this.router.navigate(['/login']);
        }
      });
    }else{
      console.log("role was not null");
      this.router.navigate(['/' + this.role]);
    }
  }
}
