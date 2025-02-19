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
  Questionnaire,
  QuestionnaireItem,
  QuestionnaireItemAnswerOption,
  QuestionnaireItemEnableWhen,
} from 'fhir/r5';
import { v4 as uuidv4 } from 'uuid';
import { QuestionType } from '../../../../shared/enums/question-types';
import { AuthService } from '../../../../core/auth/auth.service';
import { Router } from '@angular/router';

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
  ],
  templateUrl: './questionnaire.component.html',
  styleUrl: './questionnaire.component.scss',
})
export class QuestionnaireComponent implements OnInit, OnDestroy {
  questionnaire: Questionnaire;

  dateDue: string;

  constructor(private router: Router) {}

  ngOnInit(): void {
    const savedQuestionnaire = sessionStorage.getItem('questionnaire');
    if (savedQuestionnaire) {
      this.questionnaire = JSON.parse(savedQuestionnaire);
    } else {
      console.log('Unable to get specified questionnaire');
      this.router.navigate(['..']);
    }

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

  isFormValid(): boolean {
    return false;
  }

  ngOnDestroy(): void {
    sessionStorage.removeItem('questionnaire');
  }
}
