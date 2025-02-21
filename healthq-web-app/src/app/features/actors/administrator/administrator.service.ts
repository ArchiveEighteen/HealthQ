import { Injectable } from '@angular/core';
import {environment} from '../../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {User} from '../../../core/auth/user.model';

@Injectable({
  providedIn: 'root'
})
export class AdministratorService {

  url: string = environment.apiBaseUrl + '/Doctor';
  constructor(private http: HttpClient ) { }


  getAllDoctors() {
    return this.http.get(this.url + '/GetAllDoctors', {
      withCredentials: true,
    });
  }

  getNotOwnedPatients(doctorId: string) {
    return this.http.get(this.url + '/GetNotOwnedPatients/' + doctorId, {withCredentials: true});
  }
  getDoctorPatients(doctorId: string) {
    return this.http.get(this.url + '/GetAllDoctorPatients/' + doctorId, {withCredentials: true});
  }

  assignPatient(doctorId: string, patientId: string) {
    return this.http.post(this.url + '/AssignPatient/' + doctorId + '/' + patientId, null, {withCredentials: true});
  }

  removePatient(doctorId: string, patientId: string) {
    return this.http.delete(this.url + '/RemovePatient/' + doctorId + '/' + patientId);
  }
}
