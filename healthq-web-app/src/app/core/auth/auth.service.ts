import { Injectable } from '@angular/core';
import {environment} from '../../../environments/environment';
import {User} from './user.model';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  url: string = environment.apiBaseUrl + '/User'
  formData: User = new User();
  formSubmitted = false;

  isLoggedIn = false;

  constructor(private http: HttpClient) {}

  register(){
    console.log(JSON.stringify(this.formData));
    const jwtToken = localStorage.getItem('jwtToken');
    if(jwtToken){
      return this.http.post(this.url + '/Register', this.formData, { headers: { Authentication: `Bearer ${JSON.parse(jwtToken).token}` }});
    }else{
      return this.http.post(this.url + '/Register', this.formData);
    }
  }

  login(){
    console.log(JSON.stringify(this.formData));
    const jwtToken = localStorage.getItem('jwtToken');
    if(jwtToken){
      return this.http.put(this.url + '/Login', this.formData, { headers: { Authentication: `Bearer ${JSON.parse(jwtToken).token}` }});
    }else{
      return this.http.post(this.url + '/Login', this.formData);
    }
  }

  saveJwtToken(token: string){
    localStorage.setItem('jwtToken', token);
  }


  //get() method is used like example and should be deleted later along with all it's usages
  get(){
    let jwtToken = localStorage.getItem('jwtToken');
    if(jwtToken){
      return this.http.get(this.url + '/Get', {headers: { Authorization: `Bearer ${JSON.parse(jwtToken).token}`}});
    }
    return this.http.get(this.url + '/BadRequest');
  }
}
