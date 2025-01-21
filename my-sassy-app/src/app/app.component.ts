import { Component } from '@angular/core';
import {AuthenticationComponent} from './authentication/authentication.component';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-root',
  imports: [FormsModule, AuthenticationComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'HealthQ';
}
