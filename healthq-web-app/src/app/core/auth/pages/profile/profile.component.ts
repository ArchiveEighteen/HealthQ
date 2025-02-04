import { Component, signal } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCard, MatCardContent, MatCardHeader } from '@angular/material/card';
import {
  MatFormFieldModule,
  MatLabel,
  MatSuffix,
} from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAnchor, MatButton, MatIconButton } from '@angular/material/button';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule, NgForm, NgModel } from '@angular/forms';
import { AuthService } from '../../auth.service';
import { MatGridList, MatGridTile } from '@angular/material/grid-list';
import {
  MatDatepickerInput,
  MatDatepickerModule,
  MatDatepickerToggle,
} from '@angular/material/datepicker';
import { MatOption, provideNativeDateAdapter } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';
import { GenderEnum } from '../../../../shared/enums/gender-enum';
import { UserRoleEnum } from '../../../../shared/enums/user-role-enum';
import { NgForOf } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { User } from '../../user.model';
import { routes } from '../../../../app.routes';

@Component({
  selector: 'app-profile',
  providers: [provideNativeDateAdapter()],
  imports: [
    MatToolbarModule,
    MatCard,
    MatCardContent,
    MatFormFieldModule,
    MatLabel,
    MatInputModule,
    MatButton,
    MatAnchor,
    RouterLink,
    FormsModule,
    MatGridList,
    MatGridTile,
    MatDatepickerInput,
    MatDatepickerToggle,
    MatSuffix,
    MatDatepickerModule,
    MatSelect,
    MatOption,
    MatCardHeader,
    NgForOf,
    MatIconButton,
    MatIcon,
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent {
  EGender = Object.entries(GenderEnum);
  EUserRole = Object.entries(UserRoleEnum);
  readonly _currentDate = new Date();
  errorMessage: string = '';

  constructor(
    public service: AuthService,
    private router: Router)
  {
    let user = sessionStorage.getItem('user');
    if (user) {
      service.formData.userType = JSON.parse(user).userType;
      service.formData.firstName = JSON.parse(user).firstName;
      service.formData.lastName = JSON.parse(user).lastName;
      service.formData.email = JSON.parse(user).email;
      service.formData.gender = JSON.parse(user).gender;
      service.formData.birthDate = JSON.parse(user).birthDate;
      service.formData.username = JSON.parse(user).username;
      service.formData.phoneNumber = JSON.parse(user).phoneNumber;
    }else{
      console.log("User not found");
    }
  }

  onSubmit(form: NgForm) {
    this.service.formSubmitted = true;
    if (form.valid) {
      this.service.updateUser().subscribe({
        next: (data) => {
          console.log(data);
          sessionStorage.setItem('user', JSON.stringify(data));
        },
        error: (error) => {
          console.log(error);
        },
      });
    }
  }

  logout(){
    this.service.logout()
      .then(() => this.router.navigate(['/login']))
      .catch(err => console.log("Logout error:", err));
  }
}
