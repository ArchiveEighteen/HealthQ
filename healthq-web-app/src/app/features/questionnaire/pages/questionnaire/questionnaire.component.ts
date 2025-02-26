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
import { PServiceService } from '../../../actors/patient/p-service.service';
import { QuestionnaireService } from '../../questionaire.service';

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
  isReview: boolean = false;

  observations: Observation[] = [];

  dateDue: string;

  clinicalImpression: ClinicalImpression = null;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private patientService: PServiceService,
    private questionnaireService: QuestionnaireService
  ) {}

  sortObservations() {
    this.observations.sort((a, b) => {
      const orderA = this.clinicalImpression.finding.findIndex(
        (f) => f.item.reference.reference === a.id
      );
      const orderB = this.clinicalImpression.finding.findIndex(
        (f) => f.item.reference.reference === b.id
      );
      return orderA - orderB;
    });
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.questionnaire = JSON.parse(params['questionnaire']);

      if (params['isReview']) {
        this.isReview = params['isReview'] === 'true';
        this.clinicalImpression = JSON.parse(params['clinicalImpression']);

        for (let i = 0; i < this.clinicalImpression.finding.length; ++i) {
          this.questionnaireService
            .getObservationById(
              this.clinicalImpression.finding[i].item.reference.reference
            )
            .subscribe({
              next: (data) => {
                this.observations.push(data);

                if (
                  this.observations.length ===
                  this.clinicalImpression.finding.length
                ) {
                  this.sortObservations();
                }
              },
            });
        }
      }
    });

    if (!this.clinicalImpression) {
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
          reference: user.email,
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
    }

    this.dateDue = this.convertToReadableDate(
      this.questionnaire.effectivePeriod.end
    );
  }

  convertToReadableDate(dateStr: string): string {
    const date = new Date(dateStr);

    return (
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
      date.getSeconds().toString().padStart(2, '0')
    );
  }

  saveToSessionStorage() {
    sessionStorage.setItem('questionnaire', JSON.stringify(this.questionnaire));
  }

  onSubmit() {
    const now = new Date();

    this.clinicalImpression.date = now.toISOString();

    this.patientService
      .submitClinicalImpression(
        this.questionnaire.id,
        this.clinicalImpression,
        this.observations
      )
      .subscribe({
        next: (data) => {
          this.router.navigate(['..']);
        },
        error: (err) => {
          console.error('Error:', err);
        },
      });

    this.questionnaire.status = 'retired';

    this.questionnaireService.updateById(this.questionnaire).subscribe({
      error: (err) => {
        console.log(err);
      },
    });
  }

  onClose() {
    this.router.navigate(['..']);
  }

  isAnswerFormValid(): boolean {
    for (let i = 0; i < this.observations.length; i++) {
      if (
        this.observations[i].valueString === '' &&
        this.questionnaire.item[i].required === true
      ) {
        return false;
      }
    }

    return true;
  }

  ngOnDestroy(): void {
    sessionStorage.removeItem('questionnaire');
  }
}
