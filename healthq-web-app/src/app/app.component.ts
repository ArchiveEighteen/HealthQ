import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {FormsModule} from '@angular/forms';
import {HeaderComponent} from './core/layout/header/header.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, FormsModule, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'healthq-web-app';
}
