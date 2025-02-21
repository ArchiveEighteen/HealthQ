import {Component, Input} from '@angular/core';
import {MatButton} from "@angular/material/button";
import {
    MatCard,
    MatCardActions,
    MatCardContent,
    MatCardHeader,
    MatCardSubtitle,
    MatCardTitle
} from "@angular/material/card";
import {Questionnaire} from 'fhir/r5';
import {Router} from '@angular/router';
import {NgClass} from '@angular/common';

@Component({
  selector: 'app-p-questionnaire',
  imports: [
    MatCard,
    MatCardContent,
    MatCardHeader,
    MatCardSubtitle,
    MatCardTitle,
    NgClass
  ],
  templateUrl: './p-questionnaire.component.html',
  styleUrl: './p-questionnaire.component.scss'
})
export class PQuestionnaireComponent {
  @Input() questionnaire: Questionnaire;

  constructor(private router: Router) {}

  onClick() {
    sessionStorage.setItem('questionnaire', JSON.stringify(this.questionnaire));

    this.router.navigate(['./survey']);
  }
}
