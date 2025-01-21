import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {FormsModule} from '@angular/forms';
import {RegisterComponent} from './core/auth/pages/register/register.component';
import {QConstructorComponent} from './features/q-constructor/pages/q-constructor/q-constructor.component';
import {HeaderComponent} from './core/layout/header/header.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, FormsModule, RegisterComponent, QConstructorComponent, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'healthq-web-app';
}
