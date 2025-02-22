import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { QuestionnaireItemEnableWhen } from 'fhir/r5';
import { ConditionalOperators } from '../../../../shared/enums/conditional-operators';

@Component({
  selector: 'app-question-condition',
  imports: [
    FormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
  ],
  templateUrl: './question-condition.component.html',
  styleUrl: './question-condition.component.scss',
})
export class QuestionConditionComponent implements OnInit {
  @Input({ required: true }) condition: QuestionnaireItemEnableWhen;
  @Input({ required: true }) conditionCount: number;

  @Output() callSave = new EventEmitter<void>();
  @Output() callRemove = new EventEmitter<void>();

  conditionalOperators = Object.entries(ConditionalOperators);

  ngOnInit(): void {}

  onConditionalOperatorChange(event: any) {
    this.saveToSessionStorage();
  }

  saveToSessionStorage() {
    this.callSave.emit();
  }

  removeCondition() {
    this.callRemove.emit();
  }
}
