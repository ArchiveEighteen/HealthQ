import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Questionnaire } from 'fhir/r5';
import { v4 as uuidv4 } from 'uuid';

@Injectable({
  providedIn: 'root',
})
export class QuestionnaireService {
  url: string = environment.apiBaseUrl + '/Doctor';

  constructor(private http: HttpClient) {}

  addByEmail(email: string, questionnaire: Questionnaire) {
    questionnaire.id = uuidv4();
    questionnaire.publisher = email;

    return this.http.post(this.url + '/AddByEmail', questionnaire, {
      withCredentials: true,
    });
  }

  getByEmail(email: string) {
    return this.http.get(this.url + '/GetByEmail/' + email, {
      withCredentials: true,
    });
  }

  updateById(questionnaire: Questionnaire) {
    return this.http.put(this.url + '/UpdateById', questionnaire, {
      withCredentials: true,
    });
  }

  assignToPatient(patientEmail: string, questionnaire: Questionnaire) {
    return this.http.put(
      this.url + '/UpdateById/' + patientEmail,
      questionnaire,
      {
        withCredentials: true,
      }
    );
  }
}
