import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Questionnaire } from 'fhir/r5';

@Injectable({
  providedIn: 'root',
})
export class QuestionnaireService {
  url: string = environment.apiBaseUrl + '/Questionnaire';

  constructor(private http: HttpClient) {}

  addByEmail(email: string, questionnaire: Questionnaire) {
    return this.http.post(this.url + '/AddByEmail/' + email, questionnaire, {
      withCredentials: true,
    });
  }

  getByEmail(email: string) {
    return this.http.get(this.url + '/GetByEmail/' + email, {
      withCredentials: true,
    });
  }
}
