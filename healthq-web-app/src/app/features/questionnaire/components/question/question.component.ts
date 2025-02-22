import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { QuestionType } from '../../../../shared/enums/question-types';
import { v4 as uuidv4 } from 'uuid';
import {
  Extension,
  QuestionnaireItem,
  QuestionnaireItemAnswerOption,
  QuestionnaireItemEnableWhen,
} from 'fhir/r5';
import { QuestionConditionComponent } from '../question-condition/question-condition.component';

@Component({
  selector: 'app-question',
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatDividerModule,
    MatIconModule,
    MatToolbarModule,
    MatMenuModule,
    MatButtonModule,
    MatSlideToggleModule,
    MatExpansionModule,
    QuestionConditionComponent,
    MatButtonToggleModule,
  ],
  templateUrl: './question.component.html',
  styleUrl: './question.component.scss',
})
export class QuestionComponent implements OnInit {
  @Input({ required: true }) question: QuestionnaireItem;
  @Input() conditional: boolean = false;

  @Output() callSave = new EventEmitter<void>();
  @Output() callDelete = new EventEmitter<void>();

  questionTypes = Object.entries(QuestionType);
  selectedType: string;

  ngOnInit(): void {
    this.selectedType = this.getQuestionTypeValue();
    const defaultEvent = { value: this.selectedType };
    this.onQuestionTypeChange(defaultEvent);
  }

  saveToSessionStorage() {
    this.callSave.emit();
  }

  onQuestionTypeChange(event: any) {
    const selectedType = event.value;
    const questionTypeExtension = this.getQuestionTypeExtension();
    questionTypeExtension.valueString = selectedType;
    this.saveToSessionStorage();
  }

  getQuestionTypeExtension(): any {
    return (
      this.question.extension?.find(
        (ext: any) => ext.url === 'question-type'
      ) || {}
    );
  }

  getQuestionTypeValue(): string {
    const result: Extension = this.question.extension?.find(
      (ext: Extension) => ext.url === 'question-type'
    );

    return result.valueString;
  }

  deleteOption(option: QuestionnaireItemAnswerOption) {
    const index = this.question.answerOption.indexOf(option);
    if (index > -1) {
      this.question.answerOption.splice(index, 1);
    }

    this.saveToSessionStorage();
  }

  addNewOption() {
    if (!this.question.answerOption) {
      this.question.answerOption = [];
    }

    this.question.answerOption.push({
      valueString: ``,
    });

    this.saveToSessionStorage();
  }

  deleteQuestion() {
    this.callDelete.emit();
  }

  addConditionalQuestion() {
    this.question.item.push({
      linkId: uuidv4(),
      type: 'question',
      text: '',
      answerOption: [],
      item: [],
      extension: [{ url: 'question-type', valueString: '' }],
      enableWhen: [{ question: this.question.linkId, operator: 'exists' }],
      enableBehavior: 'all',
    });
  }

  onRequiredChange(event: Event) {
    event;
  }

  onAddConditionClicked() {
    const initialCondition = this.question.enableWhen.at(0);

    this.question.enableWhen.push({
      question: initialCondition.question,
      operator: 'exists',
    });
    this.saveToSessionStorage();
  }

  removeCondition(condition: QuestionnaireItemEnableWhen) {
    const index = this.question.enableWhen.findIndex((c) => c === condition);

    if (index > -1) {
      this.question.enableWhen.splice(index, 1);
      this.saveToSessionStorage();
    } else {
      console.log('Failed to remove condition!');
    }
  }

  onEnableBehaviorChange() {
    this.saveToSessionStorage();
  }
}
