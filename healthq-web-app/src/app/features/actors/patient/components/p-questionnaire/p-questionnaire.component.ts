import { Component, Input, OnInit } from '@angular/core';
import { MatButton } from '@angular/material/button';
import {
  MatCard,
  MatCardActions,
  MatCardContent,
  MatCardHeader,
  MatCardSubtitle,
  MatCardTitle,
} from '@angular/material/card';
import { Questionnaire } from 'fhir/r5';
import { Router } from '@angular/router';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-p-questionnaire',
  imports: [
    MatCard,
    MatCardContent,
    MatCardHeader,
    MatCardSubtitle,
    MatCardTitle,
    NgClass,
  ],
  templateUrl: './p-questionnaire.component.html',
  styleUrl: './p-questionnaire.component.scss',
})
export class PQuestionnaireComponent implements OnInit {
  dateAssigned: string = '';

  @Input() questionnaire: Questionnaire;

  constructor(private router: Router) {}

  ngOnInit(): void {
    const date = new Date(this.questionnaire.date);

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

  onClick() {
    if (this.questionnaire.status !== 'retired') {
      this.router.navigate(['Patient', 'questionnaire'], {
        queryParams: {
          questionnaire: JSON.stringify(this.questionnaire),
          isReview: 'false',
        },
      });
    }
  }
}
