import { Component } from '@angular/core';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatCard, MatCardContent, MatCardHeader} from '@angular/material/card';
import {MatFormFieldModule, MatLabel, MatSuffix} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatAnchor, MatButton} from '@angular/material/button';
import {RouterLink} from '@angular/router';
import {FormsModule, NgForm} from '@angular/forms';
import {AuthenticationService} from './api-connection/authentication.service';
import {MatGridList, MatGridTile} from '@angular/material/grid-list';
import {
  MatDatepickerInput,
  MatDatepickerModule,
  MatDatepickerToggle
} from '@angular/material/datepicker';
import {MatOption, provideNativeDateAdapter} from '@angular/material/core';
import {MatSelect} from '@angular/material/select';

@Component({
  selector: 'app-authentication',
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
    MatCardHeader
  ],
  templateUrl: './authentication.component.html',
  styleUrl: './authentication.component.scss'
})
export class AuthenticationComponent {

  constructor(public service: AuthenticationService) {
    service.formData.userType = "Administrator";
    service.formData.password = "123456";
    service.formData.firstName = "John";
    service.formData.lastName = "Doe";
    service.formData.email = "john@doe.com";
    service.formData.gender = "Male";
    service.formData.birthDate = new Date(2005, 1, 6, 0, 0, 0, 0);
    service.formData.username = "CoolJohn";
    service.formData.phoneNumber = "0674527417";
  }

  readonly _currentDate = new Date();
  onSubmit(form: NgForm) {
    this.service.formSubmitted = true;
    if(form.valid){
      this.service.putUser().subscribe({
        next: data =>{
          console.log(data);
        },
        error: error => {
          console.log(error);
        }
      });
    }
  }
}
