import { Component, OnInit } from '@angular/core';
import { DPatientComponent } from '../../components/d-patient/d-patient.component';
import { User } from '../../../../../core/auth/user.model';
import { QuestionnaireService } from '../../../../questionnaire/questionaire.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-d-patients-page',
  imports: [DPatientComponent],
  templateUrl: './d-patients-page.component.html',
  styleUrl: './d-patients-page.component.scss',
})
export class DPatientsPageComponent implements OnInit {
  patients: User[] = [];

  constructor(
    private constructorService: QuestionnaireService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const user: User = JSON.parse(sessionStorage.getItem('user')!);
    if (!user) {
      console.log('User is invalid!');
    }

    this.constructorService.getAllDoctorPatients(user.email).subscribe({
      next: (data) => {
        if (Array.isArray(data)) {
          this.patients = data;
        }

        console.log(this.patients);
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
}
