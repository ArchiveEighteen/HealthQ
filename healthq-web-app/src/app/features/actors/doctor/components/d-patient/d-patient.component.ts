import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { User } from '../../../../../core/auth/user.model';
import { QuestionnaireService } from '../../../../questionnaire/questionaire.service';
import { Questionnaire } from 'fhir/r5';
import { DQuestionnaireComponent } from '../d-questionnaire/d-questionnaire.component';

@Component({
  selector: 'app-d-patient',
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    DQuestionnaireComponent,
  ],
  templateUrl: './d-patient.component.html',
  styleUrl: './d-patient.component.scss',
})
export class DPatientComponent implements OnInit {
  @Input() patient: User;

  questionnaires: Questionnaire[] = [];

  questionnairesToggle: boolean = false;

  constructor(
    private router: Router,
    private constructorService: QuestionnaireService
  ) {}

  ngOnInit(): void {
    this.retrieveQuestionnaires();
  }

  onQuestionnairesClick() {
    this.questionnairesToggle = !this.questionnairesToggle;
  }

  onAssignNewClicked() {
    this.router.navigate(['Doctor', 'constructor'], {
      queryParams: { isTemplate: false, patientEmail: this.patient.email },
    });
  }

  refreshQuestionnaires() {
    this.retrieveQuestionnaires();
  }

  retrieveQuestionnaires() {
    this.questionnaires.splice(0, this.questionnaires.length);

    const user: User = JSON.parse(sessionStorage.getItem('user')!);
    if (!user) {
      console.log('User is invalid!');
    }

    this.constructorService
      .getDoctorPatientQuestionnaires(user.email, this.patient.email)
      .subscribe({
        next: (data) => {
          if (Array.isArray(data)) {
            data.forEach((d) => {
              this.questionnaires.push(JSON.parse(d));
            });
          }

          console.log(this.questionnaires);
        },
        error: (err) => {
          console.log(err);
        },
      });
  }
}
