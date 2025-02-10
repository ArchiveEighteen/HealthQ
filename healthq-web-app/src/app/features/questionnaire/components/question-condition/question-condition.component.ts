import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { QuestionnaireItemEnableWhen } from 'fhir/r5';
import { ConditionalOperators } from '../../../../shared/enums/conditional-operators';

@Component({
  selector: 'app-question-condition',
  imports: [FormsModule, MatFormFieldModule, MatSelectModule],
  templateUrl: './question-condition.component.html',
  styleUrl: './question-condition.component.scss',
})
export class QuestionConditionComponent implements OnInit {
  @Input({ required: true }) condition: QuestionnaireItemEnableWhen;

  @Output() callSave = new EventEmitter<void>();

  conditionalOperators = Object.entries(ConditionalOperators);

  ngOnInit(): void {}

  onConditionalOperatorChange(event: any) {
    this.saveToSessionStorage();
  }

  saveToSessionStorage() {
    this.callSave.emit();
  }
}
