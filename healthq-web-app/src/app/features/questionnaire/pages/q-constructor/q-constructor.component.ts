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
import { MatDatepickerModule } from '@angular/material/datepicker';
import { provideNativeDateAdapter } from '@angular/material/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Extension, Questionnaire, QuestionnaireItem } from 'fhir/r5';
import { v4 as uuidv4 } from 'uuid';
import { AuthService } from '../../../../core/auth/auth.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { User } from '../../../../core/auth/user.model';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionnaireService } from '../../questionaire.service';
import { QuestionComponent } from '../../components/question/question.component';
import { QuestionnaireTopics } from '../../../../shared/enums/questionnaire-topics';

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
    MatDatepickerModule,
    ReactiveFormsModule,
  ],
  templateUrl: './q-constructor.component.html',
  styleUrl: './q-constructor.component.scss',
  providers: [provideNativeDateAdapter()],
})
export class QConstructorComponent implements OnInit {
  @ViewChild('questionsContainer') questionsContainer!: ElementRef;

  readonly range = new FormGroup({
    start: new FormControl<Date | null>(new Date(), [
      Validators.required,
      this.minDateValidator,
    ]),
    end: new FormControl<Date | null>(null),
  });

  minDateValidator(control: FormControl) {
    const selectedDate = control.value;
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    return selectedDate && selectedDate < today
      ? { minDateInvalid: true }
      : null;
  }

  questionnaireTitle: string = 'Some Survey Title';

  questions: QuestionnaireItem[];

  questionnaire: Questionnaire;

  selectedQuestionType: string;

  questionnaireTopics = Object.entries(QuestionnaireTopics);

  questionnairePurpose: string;

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
      const formattedDate = this.getDateFormatted();

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

    if (this.questionnaire.purpose) {
      this.questionnairePurpose = this.questionnaire.purpose;
    } else {
      this.questionnairePurpose = this.questionnaireTopics[0][1];
      this.onQuestionPurposeChange();
    }

    if (this.questionnaire.effectivePeriod) {
      this.range.patchValue({
        start: new Date(this.questionnaire.effectivePeriod.start),
        end: new Date(this.questionnaire.effectivePeriod.end),
      });
    }

    this.range.statusChanges.subscribe((status) => {
      if (
        status === 'VALID' &&
        this.range.value.start &&
        this.range.value.end
      ) {
        this.onPeriodDueSelected();
      }
    });
  }

  getDateFormatted(): string {
    const now = new Date();
    return now.toISOString();
  }

  addQuestion() {
    const uuid = uuidv4();

    this.questions.push({
      id: uuid,
      extension: [
        {
          url: 'question-type',
          valueString: 'OneChoice',
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

  onQuestionPurposeChange() {
    this.questionnaire.purpose = this.questionnairePurpose;
    this.saveToSessionStorage();
  }

  getQuestionTypeValue(question: QuestionnaireItem): string {
    const result: Extension = question.extension?.find(
      (ext: Extension) => ext.url === 'question-type'
    );

    return result.valueString;
  }

  isFormValid(): boolean {
    if (!this.questionnaire.title || this.questionnaire.title.trim() === '') {
      return false;
    }

    if (
      !this.questionnaire.description ||
      this.questionnaire.description.trim() === ''
    ) {
      return false;
    }

    const today = new Date();
    today.setHours(0, 0, 0, 0);

    if (!this.range.value.start || !this.range.value.end) {
      return false;
    } else if (this.range.value.start < today) {
      return false;
    }

    if (this.questionnaire.item && this.questionnaire.item.length === 0) {
      return false;
    }

    return this.validateQuestions(this.questions);
  }

  private validateQuestions(questions: QuestionnaireItem[]): boolean {
    for (let question of questions) {
      const questionType = this.getQuestionTypeValue(question);

      if (!question.text || question.text.trim() === '') {
        return false;
      }

      if (questionType === 'OneChoice' || questionType === 'MultipleChoice') {
        if (!question.answerOption || question.answerOption.length < 2) {
          return false;
        } else {
          for (let option of question.answerOption) {
            if (!option.valueString || option.valueString.trim() === '') {
              return false;
            }
          }
        }
      }

      if (question.enableWhen.length > 0) {
        for (let condition of question.enableWhen) {
          if (!condition.operator) {
            return false;
          } else if (!condition.question) {
            return false;
          } else if (
            !condition.answerString &&
            condition.operator !== 'exists'
          ) {
            return false;
          }
        }
      }

      if (question.item && question.item.length > 0) {
        if (!this.validateQuestions(question.item)) {
          return false;
        }
      }
    }

    return true;
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

    this.questionnaire.date = this.getDateFormatted();

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

  onPeriodDueSelected() {
    this.questionnaire.effectivePeriod = {
      start: this.range.value.start.toISOString(),
      end: this.range.value.end.toISOString(),
    };
    this.saveToSessionStorage();
  }
}
