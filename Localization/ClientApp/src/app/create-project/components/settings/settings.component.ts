import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  args = 'ascending';
  reverse = false;
  constructor() { }

  ngOnInit() {
  }

}
