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
  public username = "Username";

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
            if(err.status === 401 && sessionStorage.getItem('user') != null) {
              sessionStorage.removeItem('user');
              this.retrieveUsername();
            }
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

  updateUser() {
    return this.http.put(this.url + '/UpdateUser', this.formData, { withCredentials: true })
  }

  login() {
    console.log(JSON.stringify(this.formData));
    return this.http
      .post(this.url + '/Login', this.formData, { withCredentials: true })
  }

  logout(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.http.delete(this.url + '/Logout', { withCredentials: true }).subscribe({
        next: () => {
          if(sessionStorage.getItem('user') != null)
          {
            sessionStorage.removeItem('user');
            this.retrieveUsername();
            resolve();
          }
        },
        error: (err) => {
          console.log("Logout error: ", err);
          reject(err);
        }
      })
    })
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
          if(err.status === 401 && sessionStorage.getItem('user') != null) {
              sessionStorage.removeItem('user');
              this.retrieveUsername();
          }
          resolve(null);
          console.log(err);
        }
      })
    })
  }

  retrieveUsername(){
    let user = sessionStorage.getItem('user');
    if(user === null){
      this.username = "Username";
      console.log('failed to get user from session storage');
    }
    else{
      this.username = JSON.parse(user).username;
    }
  }
}
