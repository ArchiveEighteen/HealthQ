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
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionnaireService } from '../../questionaire.service';
import { QuestionComponent } from '../../components/question/question.component';

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
    QuestionComponent,
  ],
  templateUrl: './q-constructor.component.html',
  styleUrl: './q-constructor.component.scss',
})
export class QConstructorComponent implements OnInit {
  @ViewChild('questionsContainer') questionsContainer!: ElementRef;

  questionnaireTitle: string = 'Some Survey Title';

  questions: QuestionnaireItem[];

  questionnaire: Questionnaire;

  selectedQuestionType: string;

  url: string = environment.apiBaseUrl + '/Questionnaire';

  // Route query params
  isTemplate?: boolean = false;
  patientEmail?: string;

  constructor(
    private service: AuthService,
    private http: HttpClient,
    private constructorService: QuestionnaireService,
    private router: Router,
    private route: ActivatedRoute
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
    this.route.queryParams.subscribe((params) => {
      this.isTemplate = params['isTemplate'];
      this.patientEmail = params['patientEmail'];
    });

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
        publisher: (JSON.parse(sessionStorage.getItem('user')!) as User).email,
        description: '',
        item: [],
        resourceType: 'Questionnaire',
      };
    }

    this.questions = this.questionnaire.item;
  }

  addQuestion() {
    const uuid = uuidv4();

    this.questions.push({
      id: uuid,
      extension: [
        {
          url: 'question-type',
          valueString: '',
        },
      ],
      modifierExtension: [],
      linkId: uuid,
      definition: '',
      code: [],
      prefix: '',
      text: '',
      type: 'question',
      enableWhen: [],
      required: false,
      answerOption: [],
      item: [],
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

  saveToSessionStorage() {
    sessionStorage.setItem('questionnaire', JSON.stringify(this.questionnaire));
  }

  deleteQuestion(
    questions: QuestionnaireItem[],
    targetQuestion: QuestionnaireItem
  ): boolean {
    let deleted = false;

    const deleteQuestionRecursively = (
      questions: QuestionnaireItem[],
      target: QuestionnaireItem
    ): boolean => {
      for (let i = 0; i < questions.length; i++) {
        const question = questions[i];

        if (question.linkId === targetQuestion.linkId) {
          questions.splice(i, 1);
          return true;
        }

        if (question.item.length > 0) {
          if (deleteQuestionRecursively(question.item, target)) {
            return true;
          }
        }
      }
      return false;
    };

    deleted = deleteQuestionRecursively(questions, targetQuestion);

    if (deleted) {
      this.saveToSessionStorage();
    }

    return deleted;
  }

  onSubmit() {
    const user: User = JSON.parse(sessionStorage.getItem('user')!);
    if (!user) {
      console.log('User is invalid!');
    }

    if (this.isTemplate) {
      this.questionnaire.status = 'draft';
    } else {
      this.questionnaire.status = 'active';
    }

    if (this.questionnaire.id) {
      this.constructorService.updateById(this.questionnaire).subscribe({
        next: (data) => {
          console.log(data);
        },
        error: (err) => {
          console.log(err);
        },
      });
    } else {
      this.constructorService
        .addByEmail(user.email, this.questionnaire)
        .subscribe({
          next: (data) => {
            if (this.patientEmail && this.patientEmail !== '') {
              this.constructorService
                .assignToPatient(this.patientEmail, this.questionnaire)
                .subscribe({
                  next: (data) => {
                    console.log(data);
                  },
                  error: (err) => {
                    console.log(err);
                  },
                });
            }
          },
          error: (err) => {
            console.log(err);
          },
        });
    }

    this.router.navigate(['..']);
  }

  ngOnDestroy() {
    sessionStorage.removeItem('questionnaire');
  }
}
