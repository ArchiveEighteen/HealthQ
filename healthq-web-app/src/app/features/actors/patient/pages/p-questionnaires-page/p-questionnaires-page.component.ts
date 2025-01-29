import {Component, OnInit} from '@angular/core';
import {MatIcon} from '@angular/material/icon';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatOption, MatSelect} from '@angular/material/select';
import {FormsModule} from '@angular/forms';
import {NgClass, NgForOf} from '@angular/common';
import {PServiceService} from '../../p-service.service';
import {Router} from '@angular/router';
import {Questionnaire} from 'fhir/r5';
import {MatToolbar} from '@angular/material/toolbar';
import {MatInput} from '@angular/material/input';
import { MatIconButton} from '@angular/material/button';
import {PQuestionnaireComponent} from '../../components/p-questionnaire/p-questionnaire.component';

@Component({
  selector: 'app-p-questionnaires-page',
  imports: [
    MatFormField,
    MatSelect,
    MatOption,
    MatLabel,
    FormsModule,
    NgClass,
    NgForOf,
    MatToolbar,
    MatInput,
    PQuestionnaireComponent
  ],
  templateUrl: './p-questionnaires-page.component.html',
  styleUrl: './p-questionnaires-page.component.scss'
})
export class PQuestionnairesPageComponent implements OnInit {
  availableQuestionnaires: Questionnaire[] = [];
  questionnaires: Questionnaire[] = [];
  searchQuery = '';
  selectedStatus = '';
  selectedSort = 'newest';
  selectedDoctor = '';
  doctors: string[] = [];

  statusMap: { [key: string]: string } = {
    '0': 'draft',
    '1': 'active',
    '2': 'retired',
    '3': 'unknown'
  };

  constructor(private questionnaireService: PServiceService, private router: Router) {}

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.questionnaireService.getQuestionnaires()
      .subscribe({
        next: data => {
          this.availableQuestionnaires = data;
          this.availableQuestionnaires.forEach((q) => { // @ts-ignore
            q.status = this.getStatusString(q.status)})
          this.doctors = [...new Set(data.map(q => q.publisher!))]; // Get unique doctors
          this.applyFilters();
        },
        error: err => {console.log(err)}
      });
  }

  getStatusString(status: string): string {
    return this.statusMap[status] || 'unknown';
  }

  applyFilters() {
    this.questionnaires = this.availableQuestionnaires
      .filter(q =>
        (!this.searchQuery || q.title!.toLowerCase().includes(this.searchQuery.toLowerCase())) &&
        (!this.selectedStatus || q.status === this.selectedStatus) &&
        (!this.selectedDoctor || q.publisher === this.selectedDoctor)
      )
      .sort((a, b) => {
        if (this.selectedSort === 'newest') return new Date(b.date!).getTime() - new Date(a.date!).getTime();
        if (this.selectedSort === 'oldest') return new Date(a.date!).getTime() - new Date(b.date!).getTime();
        return 0;
      });
  }
}

