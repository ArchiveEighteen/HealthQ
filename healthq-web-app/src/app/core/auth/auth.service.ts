import {inject, Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {User} from './user.model';
import {HttpClient} from '@angular/common/http';
import {BehaviorSubject, Observable} from 'rxjs';
import {CookieService} from '../../shared/services/cookie.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  url: string = environment.apiBaseUrl + '/User'
  formData: User = new User();
  formSubmitted = false;

  $isLoggedIn: boolean = false;
  constructor(private http: HttpClient, private cookieService: CookieService ) {

    let auth_token = this.cookieService.getCookie('auth_token')
    console.log('auth_token: ', auth_token);
    this.$isLoggedIn = auth_token.length > 0;

    console.log('End of constructor: ', this.$isLoggedIn);
  }

  register(){
    console.log(JSON.stringify(this.formData));
    return this.http.post(this.url + '/Register', this.formData, {withCredentials: true});
  }

  login(){
    console.log(JSON.stringify(this.formData));
    return this.http.put(this.url + '/Login', this.formData, {withCredentials: true});
  }

  //get() method is used like example and should be deleted later along with all it's usages
  get(){
      return this.http.get(this.url + '/Get', {withCredentials: true});
  }
}
