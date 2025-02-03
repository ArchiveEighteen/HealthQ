import {Component, inject} from '@angular/core';
import {AuthService} from '../auth.service';
import {Router} from '@angular/router';
import {User} from '../user.model';

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
    this.role = this.checkUserRole();

    if(this.role === null) {
      this.authService.getUserWithToken().then(role => {
        if(role != null) {
          console.log("Role was null but now dont");
          this.authService.retrieveUsername();
          this.router.navigate(['/' + role]);
        }else{
          console.log("role was null and still null");
          this.router.navigate(['/login']);
        }
      });
    }else{
      console.log("role was not null");
      this.authService.retrieveUsername();
      this.router.navigate(['/' + this.role]);
    }
  }

  checkUserRole(){
    let userString = sessionStorage.getItem('user');
    if(userString === null) return null;
    return (JSON.parse(userString) as User).userType;
  }
}
