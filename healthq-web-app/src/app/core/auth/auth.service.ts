import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { User } from './user.model';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { CookieService } from '../../shared/services/cookie.service';
import { resolve } from '@angular/compiler-cli';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  public url: string = environment.apiBaseUrl + '/User';
  public formData: User = new User();
  public formSubmitted = false;

  private isLoggedInSubject = new BehaviorSubject<boolean>(true);
  public isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(private http: HttpClient, private cookieService: CookieService) {
    this.checkAuthenticated();
  }

  checkAuthenticated(): Promise<boolean> {
    return new Promise((resolve) => {
      this.http
        .get<{ isAuthenticated: boolean }>(this.url + '/IsAuthenticated', {
          withCredentials: true,
        })
        .subscribe({
          next: (data) => {
            this.isLoggedInSubject.next(data.isAuthenticated);
            resolve(data.isAuthenticated);
            console.log(this.isLoggedIn$);
          },
          error: (err) => {
            this.isLoggedInSubject.next(false);
            resolve(false);
            console.log(this.isLoggedIn$);
          },
        });
    });
  }
  register() {
    console.log(JSON.stringify(this.formData));
    return this.http
      .post(this.url + '/Register', this.formData, { withCredentials: true })
      .pipe(tap(() => this.checkAuthenticated()));
  }

  login() {
    console.log(JSON.stringify(this.formData));
    return this.http
      .put(this.url + '/Login', this.formData, { withCredentials: true })
      .pipe(tap(() => this.checkAuthenticated()));
  }

  //get() method is used like example and should be deleted later along with all it's usages
  get() {
    return this.http
      .get(this.url + '/Get', { withCredentials: true })
      .pipe(tap(() => this.checkAuthenticated()));
  }
}
