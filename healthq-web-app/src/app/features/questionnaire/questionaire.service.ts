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

  getDoctorQuestionnaires(email: string) {
    return this.http.get(this.url + '/GetDoctorQuestionnaires/' + email, {
      withCredentials: true,
    });
  }

  getDoctorPatientQuestionnaires(doctorEmail: string, patientEmail: string) {
    return this.http.get(
      this.url +
        '/GetDoctorPatientQuestionnaires/' +
        doctorEmail +
        '/' +
        patientEmail,
      {
        withCredentials: true,
      }
    );
  }

  updateById(questionnaire: Questionnaire) {
    return this.http.put(this.url + '/UpdateById', questionnaire, {
      withCredentials: true,
    });
  }

  assignToPatient(patientEmail: string, questionnaire: Questionnaire) {
    return this.http.put(
      this.url + '/AssignToPatient/' + patientEmail,
      questionnaire,
      {
        withCredentials: true,
      }
    );
  }

  getAllDoctorPatients(email: string) {
    return this.http.get(this.url + '/GetAllDoctorPatients/' + email, {
      withCredentials: true,
    });
  }

  deleteById(questionnaire: Questionnaire) {
    return this.http.delete(this.url + '/DeleteById', {
      body: questionnaire,
      withCredentials: true,
    });
  }
}
