import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { NgClass, JsonPipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Questionnaire, ClinicalImpression } from 'fhir/r5';
import { Router } from '@angular/router';
import { QuestionnaireService } from '../../../../questionnaire/questionaire.service';

@Component({
  selector: 'app-d-questionnaire',
  imports: [MatCardModule, MatButtonModule, MatIconModule, NgClass],
  templateUrl: './d-questionnaire.component.html',
  styleUrl: './d-questionnaire.component.scss',
})
export class DQuestionnaireComponent implements OnInit, OnChanges {
  dateAssigned: string = '';
  questionnaireStatus: string = '';

  @Input() questionnaire: Questionnaire;
  @Input() patientEmail: string;
  @Input() template: boolean = false;

  @Output() questionnaireDeleted = new EventEmitter<void>();

  constructor(
    private router: Router,
    private questionnaireService: QuestionnaireService
  ) {}

  ngOnInit(): void {
    if (this.questionnaire.status === 'active') {
      this.questionnaireStatus = 'In Progress';
    } else if (this.questionnaire.status === 'retired') {
      this.questionnaireStatus = 'Submitted';
    }

    const date: Date = new Date(this.questionnaire.date);

    this.dateAssigned =
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

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['template'] || changes['patientEmail']) {
      if (!this.template && !this.patientEmail) {
        throw new Error('patientEmail is required when template is false.');
      }
      if (this.template && this.patientEmail) {
        console.warn('patientEmail will be ignored because template is true.');
        this.patientEmail = undefined; // Optionally reset patientEmail
      }
    }
  }

  onEditClick() {
    sessionStorage.setItem('questionnaire', JSON.stringify(this.questionnaire));

    this.router.navigate([' Doctor', 'constructor']);
  }

  onDeleteClick() {
    this.questionnaireService.deleteById(this.questionnaire).subscribe({
      next: (data) => {
        this.questionnaireDeleted.emit();
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  onViewResultsClicked() {
    this.questionnaireService
      .getClinicalImpressionContent(this.questionnaire.id, this.patientEmail)
      .subscribe({
        next: (data) => {
          this.router.navigate(['Doctor', 'questionnaire'], {
            queryParams: {
              questionnaire: JSON.stringify(this.questionnaire),
              isReview: 'true',
              clinicalImpression: JSON.stringify(data),
            },
          });
        },
        error: (err) => console.error(err),
      });
  }
}
