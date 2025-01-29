import { Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { Questionnaire } from 'fhir/r5';
import { Router } from '@angular/router';
import { User } from '../../../../../core/auth/user.model';

@Component({
  selector: 'app-d-patient',
  imports: [MatCardModule, MatButtonModule],
  templateUrl: './d-patient.component.html',
  styleUrl: './d-patient.component.scss',
})
export class DPatientComponent {
  @Input() patient: User;

  constructor(private router: Router) {}
}
