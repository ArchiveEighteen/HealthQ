import {
  Component,
  ElementRef,
  Input,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDividerModule } from '@angular/material/divider';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';
import { FormsModule } from '@angular/forms';
import {
  ClinicalImpression,
  Observation,
  Questionnaire,
  QuestionnaireItem,
  QuestionnaireItemAnswerOption,
  QuestionnaireItemEnableWhen,
} from 'fhir/r5';
import { v4 as uuidv4 } from 'uuid';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionComponent } from '../../components/question/question.component';
import { User } from '../../../../core/auth/user.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-questionnaire',
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatToolbarModule,
    MatIconModule,
    MatSelectModule,
    FormsModule,
    MatCheckboxModule,
    MatDividerModule,
    MatTooltipModule,
    MatMenuModule,
    QuestionComponent,
  ],
  templateUrl: './questionnaire.component.html',
  styleUrl: './questionnaire.component.scss',
})
export class QuestionnaireComponent implements OnInit, OnDestroy {
  questionnaire: Questionnaire;

  observations: Observation[] = [];

  dateDue: string;

  clinicalImpression: ClinicalImpression;

  constructor(private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.questionnaire = JSON.parse(params['questionnaire']);
    });

    const user: User = JSON.parse(sessionStorage.getItem('user')!);
    if (!user) {
      console.log('User is invalid!');
    }

    const ciid = uuidv4();
    this.clinicalImpression = {
      id: ciid,
      resourceType: 'ClinicalImpression',
      status: 'preparation',
      subject: {
        id: user.email,
      },
      finding: [],
    };

    this.questionnaire.item.forEach((item) => {
      let observation: Observation = {
        id: uuidv4(),
        resourceType: 'Observation',
        status: 'unknown',
        code: {},
        basedOn: [{ reference: item.id }],
        valueString: '',
      };

      this.observations.push(observation);

      this.clinicalImpression.finding.push({
        item: {
          reference: {
            type: 'http://hl7.org/fhir/StructureDefinition/Observation',
            reference: observation.id,
          },
        },
      });
    });

    this.route.queryParams.subscribe((params) => {
      this.questionnaire = JSON.parse(params['questionnaire']);
    });

    const date = new Date(this.questionnaire.effectivePeriod.end);

    this.dateDue =
      date.getDate().toString().padStart(2, '0') +
      '.' +
      date.getMonth().toString().padStart(2, '0') +
      '.' +
      date.getFullYear() +
      ' ' +
      date.getHours().toString().padStart(2, '0') +
      ':' +
      date.getMinutes().toString().padStart(2, '0') +
      ':' +
      date.getSeconds().toString().padStart(2, '0');
  }

  saveToSessionStorage() {
    sessionStorage.setItem('questionnaire', JSON.stringify(this.questionnaire));
  }

  onSubmit() {}

  isAnswerFormValid(): boolean {
    return false;
  }

  ngOnDestroy(): void {
    sessionStorage.removeItem('questionnaire');
  }
}
