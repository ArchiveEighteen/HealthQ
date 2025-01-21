import { Injectable } from '@angular/core';
import {environment} from '../../../environments/environment';
import {User} from './user.model';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  url: string = environment.apiBaseUrl + '/User'
  formData: User = new User();
  formSubmitted = false;

  constructor(private http: HttpClient) {}

  putUser(){
    console.log(JSON.stringify(this.formData));
    return this.http.post(this.url, this.formData);
  }
}
