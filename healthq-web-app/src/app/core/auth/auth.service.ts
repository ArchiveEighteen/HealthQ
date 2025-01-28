import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { User } from './user.model';
import { HttpClient } from '@angular/common/http';
import {Router} from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  public url: string = environment.apiBaseUrl + '/User';
  public formData: User = new User();
  public formSubmitted = false;

  constructor(private http: HttpClient) {
  }

  checkAuthenticated(): Promise<boolean> {
    return new Promise((resolve) => {
      this.http
        .get<{ isAuthenticated: boolean }>(this.url + '/IsAuthenticated', {
          withCredentials: true,
        })
        .subscribe({
          next: (data) => {
            resolve(data.isAuthenticated);
          },
          error: (err) => {
            resolve(false);
          },
        });
    });
  }
  register() {
    console.log(JSON.stringify(this.formData));
    return this.http
      .post(this.url + '/Register', this.formData, { withCredentials: true })
  }

  login() {
    console.log(JSON.stringify(this.formData));
    return this.http
      .put(this.url + '/Login', this.formData, { withCredentials: true })
  }

  //get() method is used like example and should be deleted later along with all it's usages
  get() {
    return this.http
      .get(this.url + '/Get', { withCredentials: true })
  }

  getUserWithToken(): Promise<string | null>{
    return new Promise((resolve) => {
      this.http.get(this.url + '/GetUser', {withCredentials: true}).subscribe({
        next: (data) => {
          sessionStorage.setItem('user', JSON.stringify(data));
          resolve ((data as User).userType);
        },
        error: (err) => {
          resolve(null);
          console.log(err);
        }
      })
    })
  }

  checkUserRole(){
    let userString = sessionStorage.getItem('user');
    if(userString === null) return null;
    return (JSON.parse(userString) as User).userType;
  }
}
