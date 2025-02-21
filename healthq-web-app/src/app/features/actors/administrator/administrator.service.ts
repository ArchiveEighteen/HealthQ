import { Injectable } from '@angular/core';
import {environment} from '../../../../environments/environment';
import {HttpClient} from '@angular/common/http';

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
}
