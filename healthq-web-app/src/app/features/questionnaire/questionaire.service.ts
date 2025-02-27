import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ClinicalImpression, Observation, Questionnaire } from 'fhir/r5';
import { v4 as uuidv4 } from 'uuid';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class QuestionnaireService {
  url: string = environment.apiBaseUrl + '/Doctor';

  constructor(private http: HttpClient) {}

  addByEmail(email: string, questionnaire: Questionnaire) {
    return this.http.post(this.url + '/AddByEmail', questionnaire, {
      withCredentials: true,
    });
  }

  getDoctorTemplates(email: string) {
    return this.http.get(this.url + '/GetDoctorTemplates/' + email, {
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

  getClinicalImpressionContent(questionnaireId: string, patientId: string) {
    return this.http.get<ClinicalImpression>(
      this.url +
        '/GetClinicalImpressionContent/' +
        questionnaireId +
        '/' +
        patientId,
      { withCredentials: true }
    );
  }

  getObservationById(observationId: string) {
    return this.http.get<Observation>(
      this.url + '/GetObservationById/' + observationId,
      {
        withCredentials: true,
      }
    );
  }

  saveTemplate(questionnaire: Questionnaire) {
    return this.http.post(this.url + '/SaveTemplate', questionnaire, {withCredentials: true});
  }

  deleteTemplate(id: string) {
    return this.http.delete(this.url + '/DeleteTemplate/' + id, {withCredentials: true});
  }
}
