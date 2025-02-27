import {Component, Inject, OnInit} from '@angular/core';
import {MatButton} from "@angular/material/button";
import {MatIcon} from "@angular/material/icon";
import {MatList, MatListItem} from "@angular/material/list";
import {MatSidenav, MatSidenavContainer, MatSidenavContent} from "@angular/material/sidenav";
import {ActivatedRoute, Router, RouterLink, RouterLinkActive, RouterOutlet} from "@angular/router";
import {FormsModule} from '@angular/forms';

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
    RouterLink,
    FormsModule
  ],
  templateUrl: './a-main-page.component.html',
  styleUrl: './a-main-page.component.scss'
})
export class AMainPageComponent implements OnInit {

  constructor(public router: Router, public route: ActivatedRoute){
  }

  ngOnInit(): void {

    if(this.router.url === "/Administrator"){
      this.router.navigate(['./doctors'], {relativeTo: this.route});
    }
  }
}
