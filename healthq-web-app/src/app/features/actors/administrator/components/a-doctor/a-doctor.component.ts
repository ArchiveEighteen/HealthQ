import {Component, Input} from '@angular/core';
import {MatCard, MatCardContent, MatCardHeader, MatCardSubtitle, MatCardTitle} from "@angular/material/card";
import {Router} from '@angular/router';
import {User} from '../../../../../core/auth/user.model';

@Component({
  selector: 'app-a-doctor',
    imports: [
        MatCard,
        MatCardContent,
        MatCardHeader,
        MatCardSubtitle,
        MatCardTitle
    ],
  templateUrl: './a-doctor.component.html',
  styleUrl: './a-doctor.component.scss'
})
export class ADoctorComponent {
  @Input() doctor: User;

  constructor(private router: Router) {}

  onClick() {
    sessionStorage.setItem('doctor', JSON.stringify(this.doctor));

    this.router.navigate(['Administrator/doctor', this.doctor.email]);
  }
}
