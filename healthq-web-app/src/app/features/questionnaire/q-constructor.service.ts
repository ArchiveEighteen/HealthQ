import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { User } from '../../core/auth/user.model';
import { Questionnaire } from 'fhir/r5';

@Injectable({
  providedIn: 'root',
})
export class QConstructorService {
  url: string = environment.apiBaseUrl + '/Questionnaire';

  constructor(private http: HttpClient) {}

  addByEmail(questionnaire: Questionnaire) {
    return this.http.post(this.url + '/AddByEmail', questionnaire, {
      withCredentials: true,
    });
  }
}
