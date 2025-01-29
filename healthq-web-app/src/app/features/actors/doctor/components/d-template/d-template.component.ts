import { Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { Questionnaire } from 'fhir/r5';
import { Router } from '@angular/router';

@Component({
  selector: 'app-d-template',
  imports: [MatCardModule, MatButtonModule],
  templateUrl: './d-template.component.html',
  styleUrl: './d-template.component.scss',
})
export class DTemplateComponent {
  @Input() questionnaire: Questionnaire;

  constructor(private router: Router) {}

  onEditClick() {
    sessionStorage.setItem('questionnaire', JSON.stringify(this.questionnaire));

    this.router.navigate(['/Doctor/constructor']);
  }
}
