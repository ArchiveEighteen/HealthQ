import {Component, OnInit} from '@angular/core';
import {MatDialogRef} from '@angular/material/dialog';
import {MatTab, MatTabChangeEvent, MatTabGroup} from '@angular/material/tabs';
import {MatCard, MatCardTitle} from '@angular/material/card';
import {MatToolbar} from '@angular/material/toolbar';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatIcon} from '@angular/material/icon';
import {NgForOf, NgIf} from '@angular/common';
import {MatInput} from '@angular/material/input';
import {FormsModule} from '@angular/forms';
import {Router} from '@angular/router';
import {QuestionnaireService} from '../../questionaire.service';
import {User} from '../../../../core/auth/user.model';
import {Questionnaire} from 'fhir/r5';
import {MatCell} from '@angular/material/table';
import {MatIconButton} from '@angular/material/button';

@Component({
  selector: 'app-q-template-selector',
  imports: [
    MatCard,
    MatToolbar,
    MatFormField,
    MatIcon,
    MatTabGroup,
    MatTab,
    NgForOf,
    MatCardTitle,
    MatInput,
    FormsModule,
    MatLabel,
    MatCell,
    MatIconButton,
    NgIf
  ],
  templateUrl: './q-template-selector.component.html',
  styleUrl: './q-template-selector.component.scss'
})
export class QTemplateSelectorComponent implements OnInit {

  searchQuery: string = '';
  activeTab: string = 'my';

  allTemplates: Questionnaire[] = []
  myTemplates: Questionnaire[] = []
  sharedTemplates: Questionnaire[] = []

  constructor(
    private dialogRef: MatDialogRef<QTemplateSelectorComponent>,
    private router: Router,
    private service: QuestionnaireService) {}

  ngOnInit() {
    const user: User = this.getUser();

    this.service.getDoctorTemplates(user.email).subscribe({
      next: result => {
        if(Array.isArray(result)) {
          result.forEach((d) => {
            const template: Questionnaire = JSON.parse(d);
            this.allTemplates.push(template);

            if (template.publisher === user.email)
              this.myTemplates.push(template);
            else
              this.sharedTemplates.push(template);
          });
        }
      },
      error: err => console.log(err)
    })
  }

  getUser(): User {
    const user: User = JSON.parse(sessionStorage.getItem('user')!);
    if (!user) {
      console.log('User is invalid!');
    }

    return user;
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  onTabChange(event: MatTabChangeEvent): void {
    this.activeTab = event.tab.textLabel.toLowerCase();
  }

  filteredTemplates(questionnaires: Questionnaire[]) {
    return questionnaires.filter(t =>
      t.title.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
  }

  removeTemplate(id: string) {
    this.service.deleteTemplate(id).subscribe({
      next: () => {
        console.log('Delete Template: ', id);

        this.allTemplates = this.allTemplates.filter(t => t.id !== id);
        this.sharedTemplates = this.sharedTemplates.filter(t => t.id !== id);
        this.myTemplates = this.myTemplates.filter(t => t.id !== id);
      },
      error: err => console.log(err)
    })
  }

  selectTemplate(template: Questionnaire) {
    template.id = null;
    sessionStorage.setItem('questionnaire', JSON.stringify(template));
    this.dialogRef.close();
  }

  canDelete(template: Questionnaire): boolean {
    const user: User = this.getUser();

    return user.userType === 'Administrator' || template.publisher === user.email;
  }
}
