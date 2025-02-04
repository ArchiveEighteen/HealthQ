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
  selector: 'app-register',
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
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  EGender = Object.entries(GenderEnum);
  EUserRole = Object.entries(UserRoleEnum);
  readonly _currentDate = new Date();
  hidePassword = signal(true);
  confirmPassword = '';
  errorMessage: string = '';

  constructor(
    public service: AuthService,
    private router: Router,
    public route: ActivatedRoute
  ) {
    service.formData.userType = UserRoleEnum.Patient;
    service.formData.password = "123456";
    service.formData.firstName = "John";
    service.formData.lastName = "Doe";
    service.formData.email = "john@doe.com";
    service.formData.gender = GenderEnum.Male;
    service.formData.birthDate = new Date(2005, 1, 6, 0, 0, 0, 0);
    service.formData.username = "CoolJohn";
    service.formData.phoneNumber = "0674527417";
  }

  changeButtonVisibility(event: MouseEvent) {
    this.hidePassword.set(!this.hidePassword());
    event.stopPropagation();
  }
  isPasswordMatch(
    event: FocusEvent,
    password: NgModel,
    confirmPassword: NgModel
  ) {
    if (password.value != confirmPassword.value) {
      password.control.setErrors({ mismatch: true });
      confirmPassword.control.setErrors({ mismatch: true });
    } else {
      password.control.setErrors(null);
      confirmPassword.control.setErrors(null);
    }
    event.stopPropagation();
  }
  onSubmit(form: NgForm) {
    this.service.formSubmitted = true;
    if (form.valid) {
      this.service.register().subscribe({
        next: (data) => {
          console.log(data);
          this.service.formData = new User();
          sessionStorage.setItem('user', JSON.stringify(data));
          this.router.navigate([`/${(data as User).userType}`]);
        },
        error: (error) => {
          console.log(error);
        },
      });
    }
  }
}
