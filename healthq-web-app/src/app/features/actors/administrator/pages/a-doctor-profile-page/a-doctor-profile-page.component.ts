import {Component, OnInit} from '@angular/core';
import {AdministratorService} from '../../administrator.service';
import {FormsModule} from '@angular/forms';
import {NgForOf} from '@angular/common';
import {MatOption} from '@angular/material/core';
import {MatFormField, MatLabel, MatSelect} from '@angular/material/select';
import {MatCard} from '@angular/material/card';
import {
  MatCell, MatCellDef, MatColumnDef,
  MatHeaderCell,
  MatHeaderCellDef,
  MatHeaderRow,
  MatHeaderRowDef,
  MatRow,
  MatRowDef, MatTable
} from '@angular/material/table';
import {MatButton} from '@angular/material/button';
import {User} from '../../../../../core/auth/user.model';

@Component({
  selector: 'app-a-doctor-profile-page',
  imports: [
    FormsModule,
    NgForOf,
    MatOption,
    MatSelect,
    MatLabel,
    MatFormField,
    MatCard,
    MatRow,
    MatHeaderRow,
    MatRowDef,
    MatHeaderRowDef,
    MatCell,
    MatHeaderCell,
    MatHeaderCellDef,
    MatCellDef,
    MatButton,
    MatColumnDef,
    MatTable
  ],
  templateUrl: './a-doctor-profile-page.component.html',
  styleUrl: './a-doctor-profile-page.component.scss'
})
export class ADoctorProfilePageComponent implements OnInit {

  doctor: User;
  patients: User[] = [];
  allPatients: User[] = []; // For assigning new patients
  selectedPatientId: string = '';
  displayedColumns: string[] = ['name', 'email', 'actions'];

  constructor(private adminService: AdministratorService) {}

  ngOnInit() {
    this.doctor = JSON.parse(sessionStorage.getItem('doctor')!);
    this.loadDoctorsPatients();
    this.loadNotOwnedPatients();
  }


  loadNotOwnedPatients(){
    this.adminService.getNotOwnedPatients(this.doctor.email).subscribe({
      next: patients => {
        if(Array.isArray(patients)){
          this.allPatients = patients;
        }
      },
      error: err => console.log(err)
    })
  }
  loadDoctorsPatients() {
    this.adminService.getDoctorPatients(this.doctor.email).subscribe({
      next: patients =>
      {
        if(Array.isArray(patients)) {
          this.patients = patients;
        }
      },
      error: error => console.log(error)
    });
  }

  assignPatient() {
    if (this.selectedPatientId) {
      this.adminService.assignPatient(this.doctor.email, this.selectedPatientId).subscribe(() => {
        this.loadDoctorsPatients();
        this.loadNotOwnedPatients();
        this.selectedPatientId = '';
      });
    }
  }

  removePatient(patientId: string) {
    this.adminService.removePatient(this.doctor.email, patientId).subscribe(() => {
      this.loadDoctorsPatients();
      this.loadNotOwnedPatients();
    });
  }
}
