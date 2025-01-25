import {Component, signal} from '@angular/core';
import {FormsModule, NgForm, ReactiveFormsModule} from "@angular/forms";
import {MatAnchor, MatButton, MatIconButton} from "@angular/material/button";
import {MatCard, MatCardContent, MatCardHeader} from "@angular/material/card";
import {MatFormField, MatLabel, MatSuffix} from "@angular/material/form-field";
import {MatIcon} from "@angular/material/icon";
import {MatInput} from "@angular/material/input";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {AuthService} from '../../auth.service';

@Component({
  selector: 'app-login',
    imports: [
        FormsModule,
        MatAnchor,
        MatButton,
        MatCard,
        MatCardContent,
        MatCardHeader,
        MatFormField,
        MatIcon,
        MatIconButton,
        MatInput,
        MatLabel,
        MatSuffix,
        ReactiveFormsModule,
        RouterLink
    ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  hidePassword = signal(true);
  constructor(public service: AuthService, private router: Router, private route: ActivatedRoute) {
  }
  changeButtonVisibility(event: MouseEvent) {
    this.hidePassword.set(!this.hidePassword());
    event.stopPropagation();
  }
  onSubmit(form: NgForm) {
    this.service.formSubmitted = true;
    if(form.valid) {
      this.service.login().subscribe({
        next: data => {
          this.service.isLoggedIn = true;
          this.service.saveJwtToken(JSON.stringify(data));
          this.router.navigate(['/']);
          console.log(data);
        },
        error:err => {console.log(err);},
      })
    }
  }

}
