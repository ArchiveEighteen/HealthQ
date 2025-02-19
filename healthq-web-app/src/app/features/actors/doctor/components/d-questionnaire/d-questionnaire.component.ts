import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Questionnaire } from 'fhir/r5';
import { Router } from '@angular/router';
import { QuestionnaireService } from '../../../../questionnaire/questionaire.service';

@Component({
  selector: 'app-d-questionnaire',
  imports: [MatCardModule, MatButtonModule, MatIconModule],
  templateUrl: './d-questionnaire.component.html',
  styleUrl: './d-questionnaire.component.scss',
})
export class DQuestionnaireComponent implements OnChanges {
  @Input() questionnaire: Questionnaire;
  @Input() patientEmail: string;
  @Input() template: boolean = false;

  @Output() questionnaireDeleted = new EventEmitter<void>();

  constructor(
    private router: Router,
    private questionnaireService: QuestionnaireService
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['template'] || changes['patientEmail']) {
      if (!this.template && !this.patientEmail) {
        throw new Error('patientEmail is required when template is false.');
      }
      if (this.template && this.patientEmail) {
        console.warn('patientEmail will be ignored because template is true.');
        this.patientEmail = undefined; // Optionally reset patientEmail
      }
    }
  }

  onEditClick() {
    sessionStorage.setItem('questionnaire', JSON.stringify(this.questionnaire));

    this.router.navigate(['/Doctor/constructor']);
  }

  onDeleteClick() {
    this.questionnaireService.deleteById(this.questionnaire).subscribe({
      next: (data) => {
        console.log('Deleted successfully!');
        this.questionnaireDeleted.emit();
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
}
