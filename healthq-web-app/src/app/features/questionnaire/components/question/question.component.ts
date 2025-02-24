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
import { MatRadioChange, MatRadioModule } from '@angular/material/radio';
import {
  MatCheckboxChange,
  MatCheckboxModule,
} from '@angular/material/checkbox';
import { QuestionType } from '../../../../shared/enums/question-types';
import { UUIDTypes, v4 as uuidv4 } from 'uuid';
import {
  Extension,
  QuestionnaireItem,
  QuestionnaireItemAnswerOption,
  QuestionnaireItemEnableWhen,
  Observation,
} from 'fhir/r5';
``;
import { FileUploadComponent } from '../../../../shared/components/file-upload/file-upload.component';
import { User } from '../../../../core/auth/user.model';

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
    MatRadioModule,
    MatCheckboxModule,
    MatButtonToggleModule,
    FileUploadComponent,
  ],
  templateUrl: './question.component.html',
  styleUrl: './question.component.scss',
})
export class QuestionComponent implements OnInit {
  @Input({ required: true }) question: QuestionnaireItem;
  @Input({ required: true }) observation: Observation;

  @Output() callSave = new EventEmitter<void>();

  questionTypes = Object.entries(QuestionType);

  selectedCheckboxValues: string[] = [];

  oid: string = uuidv4();

  ngOnInit(): void {}

  onRadioSelectionChange(event: MatRadioChange) {
    const selectedValue = event.value;
    this.observation.valueString = selectedValue;
  }

  onCheckboxChange(event: MatCheckboxChange, value: string): void {
    if (event.checked) {
      this.selectedCheckboxValues.push(value);
    } else {
      this.selectedCheckboxValues = this.selectedCheckboxValues.filter(
        (v) => v !== value
      );
    }

    this.observation.valueString = JSON.stringify(this.selectedCheckboxValues);

    console.log('Selected values:', this.observation.valueString);
  }

  getQuestionTypeValue(): string {
    const result: Extension = this.question.extension?.find(
      (ext: Extension) => ext.url === 'question-type'
    );

    const stringValue = result.valueString;

    return result.valueString;
  }
}
