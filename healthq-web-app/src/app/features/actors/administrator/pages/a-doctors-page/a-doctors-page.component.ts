import {Component, OnInit} from '@angular/core';
import {User} from '../../../../../core/auth/user.model';
import {AdministratorService} from '../../administrator.service';
import {Router} from '@angular/router';
import {FormsModule} from '@angular/forms';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {MatToolbar} from '@angular/material/toolbar';
import {NgForOf} from '@angular/common';
import {ADoctorComponent} from '../../components/a-doctor/a-doctor.component';

@Component({
  selector: 'app-a-doctors-page',
  imports: [
    FormsModule,
    MatFormField,
    MatInput,
    MatLabel,
    MatToolbar,
    NgForOf,
    ADoctorComponent
  ],
  templateUrl: './a-doctors-page.component.html',
  styleUrl: './a-doctors-page.component.scss'
})
export class ADoctorsPageComponent implements OnInit {

  searchQuery = '';
  allDoctors: User[] = [];
  doctors: User[] = [];

  constructor(private adminService: AdministratorService, private router: Router) {}

  ngOnInit(): void {
    this.adminService.getAllDoctors().subscribe({
      next: (data) => {
        if (Array.isArray(data)) {
          this.allDoctors = data;
          this.applyFilters();
        }
      },
      error: (err) => {
        console.log(err)
      }
    })
  }

  public applyFilters() {
    this.doctors = this.allDoctors
      .filter(q =>
        (
          !this.searchQuery ||
          q.firstName!.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
          q.lastName!.toLowerCase().includes(this.searchQuery.toLowerCase())
        )
      )
  }
}
