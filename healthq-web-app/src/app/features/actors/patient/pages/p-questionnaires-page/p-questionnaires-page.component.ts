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

@Component({
  selector: 'app-p-questionnaires-page',
  imports: [
    MatIcon,
    MatFormField,
    MatSelect,
    MatOption,
    MatLabel,
    FormsModule,
    NgClass,
    NgForOf,
    MatToolbar,
    MatInput,
    MatIconButton
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

  constructor(private questionnaireService: PServiceService, private router: Router) {}

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.questionnaireService.getQuestionnaires()
      .subscribe({
        next: data => {
          this.availableQuestionnaires = data;
          this.doctors = [...new Set(data.map(q => q.publisher!))]; // Get unique doctors
          this.applyFilters();
        },
        error: err => {console.log(err)}
      });
  }

  applyFilters() {
    this.questionnaires = this.availableQuestionnaires
      .filter(q =>
        (!this.searchQuery || q.name!.toLowerCase().includes(this.searchQuery.toLowerCase())) &&
        (!this.selectedStatus || q.status === this.selectedStatus) &&
        (!this.selectedDoctor || q.publisher === this.selectedDoctor)
      )/*
      .sort((a, b) => {
        if (this.selectedSort === 'newest') return new Date(b.date!).getTime() - new Date(a.date!).getTime();
        if (this.selectedSort === 'oldest') return new Date(a.date!).getTime() - new Date(b.date!).getTime();
        return 0;
      })*/;
  }

  openSurvey(id: string) {
    this.router.navigate(['/survey', id]);
  }
}
