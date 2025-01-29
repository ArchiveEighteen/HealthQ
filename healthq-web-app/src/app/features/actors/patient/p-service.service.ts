import { Injectable } from '@angular/core';
import {environment} from '../../../../environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {User} from '../../../core/auth/user.model';
import {Questionnaire} from 'fhir/r5';

@Injectable({
  providedIn: 'root'
})
export class PServiceService {
  public url: string = environment.apiBaseUrl + '/Patient';

  constructor(private http: HttpClient) { }

  getQuestionnaires(){
    let email: string = (JSON.parse(sessionStorage.getItem('user')!) as User).email;
    return this.http.get<Questionnaire[]>(this.url + '/GetQuestionnaires/' + email, {withCredentials: true});
  }
}
