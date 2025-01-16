import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioButton } from '@angular/material/radio';
import { MatDividerModule } from '@angular/material/divider';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FormsModule } from '@angular/forms';
import {
  Questionnaire,
  QuestionnaireItem,
  QuestionnaireItemAnswerOption,
} from 'fhir/r5';
import { v4 as uuidv4 } from 'uuid';
import { QuestionType } from '../../enums/question-types';
import { UiQuestionnaire } from '../../classes/ui-questionnaire';

@Component({
  selector: 'app-q-constructor',
  standalone: true,
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
    MatRadioButton,
    MatDividerModule,
    MatTooltipModule,
  ],
  templateUrl: './q-constructor.component.html',
  styleUrl: './q-constructor.component.scss',
})
export class QConstructorComponent implements OnInit {
  questionTypes = Object.entries(QuestionType);

  questionnaireTitle: string = 'Some Survey Title';

  uiQuestionnaire: UiQuestionnaire;

  selectedQuestionType: string;

  ngOnInit(): void {
    const savedQuestionnaire = localStorage.getItem('questionnaire');
    if (savedQuestionnaire) {
      this.uiQuestionnaire = JSON.parse(savedQuestionnaire);
    } else {
      this.uiQuestionnaire = {
        questionnaire: {
          title: '',
          experimental: true,
          status: 'draft',
          date: new Date().toLocaleString(),
          publisher: 'Author',
          description: '',
          item: [],
          resourceType: 'Questionnaire',
        },
        type: QuestionType.OneChoice,
      };
    }

    // Automatically set the first option as selected
    if (this.questionTypes.length > 0) {
      this.selectedQuestionType = this.questionTypes[0][0];
    }
  }

  addQuestion() {
    this.uiQuestionnaire.questionnaire.item.push({
      linkId: uuidv4(),
      type: 'question',
      text: '',
      answerOption: [],
    });

    this.saveToLocalStorage();
  }

  onQuestionChange(question: QuestionnaireItem) {
    this.saveToLocalStorage();
  }

  saveToLocalStorage() {
    localStorage.setItem('questionnaire', JSON.stringify(this.uiQuestionnaire));
  }

  deleteQuestion(question: QuestionnaireItem) {
    const index = this.uiQuestionnaire.questionnaire.item.indexOf(question);
    if (index > -1) {
      this.uiQuestionnaire.questionnaire.item.splice(index, 1);
      this.saveToLocalStorage();
    } else {
      console.log('Failed to remove question!');
    }
  }

  addNewOption(question: QuestionnaireItem) {
    if (!question.answerOption) {
      question.answerOption = [];
    }

    question.answerOption.push({
      valueString: ``,
    });

    this.saveToLocalStorage();
  }

  deleteOption(
    question: QuestionnaireItem,
    option: QuestionnaireItemAnswerOption
  ) {
    const index = question.answerOption.indexOf(option);
    if (index > -1) {
      question.answerOption.splice(index, 1);
    }

    this.saveToLocalStorage();
  }
}
