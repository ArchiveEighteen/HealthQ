import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
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
  Extension,
  Questionnaire,
  QuestionnaireItem,
  QuestionnaireItemAnswerOption,
  QuestionnaireItemEnableWhen,
} from 'fhir/r5';
import { v4 as uuidv4 } from 'uuid';
import { QuestionType } from '../../../../shared/enums/question-types';
import { AuthService } from '../../../../core/auth/auth.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { User } from '../../../../core/auth/user.model';
import { QuestionnaireComponent } from '../questionnaire/questionnaire.component';
import { routes } from '../../../../app.routes';
import { Router } from '@angular/router';
import { QuestionnaireService } from '../../questionaire.service';

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
    MatDividerModule,
    MatTooltipModule,
    MatMenuModule,
  ],
  templateUrl: './q-constructor.component.html',
  styleUrl: './q-constructor.component.scss',
})
export class QConstructorComponent implements OnInit {
  @ViewChild('questionsContainer') questionsContainer!: ElementRef;

  questionTypes = Object.entries(QuestionType);

  questionnaireTitle: string = 'Some Survey Title';

  questions: QuestionnaireItem[];

  questionnaire: Questionnaire;

  selectedQuestionType: string;

  url: string = environment.apiBaseUrl + '/Questionnaire';

  constructor(
    private service: AuthService,
    private http: HttpClient,
    private constructorService: QuestionnaireService,
    private router: Router
  ) {
    service.get().subscribe({
      next: (data) => {
        console.log(data);
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  ngOnInit(): void {
    const savedQuestionnaire = sessionStorage.getItem('questionnaire');
    if (savedQuestionnaire) {
      this.questionnaire = JSON.parse(savedQuestionnaire);
    } else {
      const now = new Date();
      const offset = -now.getTimezoneOffset(); // Get timezone offset in minutes
      const sign = offset >= 0 ? '+' : '-'; // Determine the sign for the offset
      const hoursOffset = String(Math.abs(Math.floor(offset / 60))).padStart(
        2,
        '0'
      ); // Hours part
      const minutesOffset = String(Math.abs(offset % 60)).padStart(2, '0'); // Minutes part

      const formattedDate = `${now.getFullYear()}-${String(
        now.getMonth() + 1
      ).padStart(2, '0')}-${String(now.getDate()).padStart(2, '0')}T${String(
        now.getHours()
      ).padStart(2, '0')}:${String(now.getMinutes()).padStart(2, '0')}:${String(
        now.getSeconds()
      ).padStart(2, '0')}${sign}${hoursOffset}:${minutesOffset}`;

      this.questionnaire = {
        title: '',
        status: 'draft',
        date: formattedDate,
        publisher: 'Author',
        description: '',
        item: [],
        resourceType: 'Questionnaire',
      };
    }

    // Automatically set the first option as selected
    if (this.questionTypes.length > 0) {
      this.selectedQuestionType = this.questionTypes[0][0];
    }

    // @ts-ignore
    this.questions = this.questionnaire.item;
  }

  addQuestion() {
    this.questions.push({
      linkId: uuidv4(),
      type: 'question',
      text: '',
      answerOption: [],
      extension: [{ url: 'question-type', valueString: '' }],
    });

    // Scroll to the bottom of the container
    setTimeout(() => {
      this.questionsContainer.nativeElement.scrollTo({
        top: this.questionsContainer.nativeElement.scrollHeight,
        behavior: 'smooth',
      });
    }, 0);

    this.saveToSessionStorage();
  }

  onQuestionTypeChange(question: any, event: any) {
    const selectedType = event.value;
    const questionTypeExtension = this.getQuestionTypeExtension(question);
    questionTypeExtension.valueString = selectedType;
    this.saveToSessionStorage();
  }

  saveToSessionStorage() {
    sessionStorage.setItem('questionnaire', JSON.stringify(this.questionnaire));
  }

  deleteQuestion(question: QuestionnaireItem) {
    const index = this.questions.indexOf(question);
    if (index > -1) {
      this.questions.splice(index, 1);
      this.saveToSessionStorage();
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

    this.saveToSessionStorage();
  }

  deleteOption(
    question: QuestionnaireItem,
    option: QuestionnaireItemAnswerOption
  ) {
    // @ts-ignore
    const index = question.answerOption.indexOf(option);
    if (index > -1) {
      // @ts-ignore
      question.answerOption.splice(index, 1);
    }

    this.saveToSessionStorage();
  }

  addConditionalQuestion(parentQuestion: QuestionnaireItem) {
    this.questions.push({
      linkId: uuidv4(),
      type: 'question',
      text: '',
      answerOption: [],
      enableWhen: [{ question: parentQuestion.linkId, operator: 'exists' }],
    });
  }

  onSubmit() {
    const user: User = JSON.parse(sessionStorage.getItem('user')!);
    if (!user) {
      console.log('User is invalid!');
    }

    this.questionnaire.status = 'active';

    this.constructorService
      .addByEmail(user.email, this.questionnaire)
      .subscribe({
        next: (data) => {
          console.log(data);
        },
        error: (err) => {
          console.log(err);
        },
      });

    this.router.navigate(['/Doctor']);
    sessionStorage.removeItem('questionnaire');
  }

  getQuestionTypeExtension(question: QuestionnaireItem): any {
    return (
      question.extension?.find((ext: any) => ext.url === 'question-type') || {}
    );
  }

  getQuestionTypeValue(question: QuestionnaireItem): any {
    const result: Extension = question.extension?.find(
      (ext: Extension) => ext.url === 'question-type'
    ) || { url: '' };

    return result.valueString;
  }
}
