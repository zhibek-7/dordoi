import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css']
})
export class AdministrationComponent implements OnInit {
  showSettingsComponent = true;
  showFoldersComponent = false;
  showUsersListComponent = false;
  showServersComponent = false;
  showEventsComponent = false;
  showAuthComponent = false;
  showImportComponent = false;

  constructor() { }

  ngOnInit() {
  }

  showMainContent(type: number) {
    this.showSettingsComponent = (type === 1);
    this.showFoldersComponent = (type === 2);
    this.showUsersListComponent = (type === 3);
    this.showServersComponent = (type === 4);
    this.showEventsComponent = (type === 5);
    this.showAuthComponent = (type === 6);
    this.showImportComponent = (type === 7);
  }
 }
