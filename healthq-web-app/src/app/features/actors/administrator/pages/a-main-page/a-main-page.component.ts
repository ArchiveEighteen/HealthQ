import { Component } from '@angular/core';
import {MatButton} from "@angular/material/button";
import {MatIcon} from "@angular/material/icon";
import {MatList, MatListItem} from "@angular/material/list";
import {MatSidenav, MatSidenavContainer, MatSidenavContent} from "@angular/material/sidenav";
import {RouterLink, RouterLinkActive, RouterOutlet} from "@angular/router";

@Component({
  selector: 'app-a-main-page',
  imports: [
    MatButton,
    MatIcon,
    MatList,
    MatListItem,
    MatSidenav,
    MatSidenavContainer,
    MatSidenavContent,
    RouterLinkActive,
    RouterOutlet,
    RouterLink
  ],
  templateUrl: './a-main-page.component.html',
  styleUrl: './a-main-page.component.scss'
})
export class AMainPageComponent {

}
