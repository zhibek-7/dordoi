import { Component, OnInit } from '@angular/core';
import { ProjectsService } from '../services/projects.service';
import { Project } from '../models/Project';
import * as moment from 'moment';
moment.locale('ru');

@Component({
  selector: 'app-user-account',
  templateUrl: './user-account.component.html',
  styleUrls: ['./user-account.component.css'],
  providers: [ProjectsService]
})
export class UserAccountComponent implements OnInit {

  projectsArr: Project[];
  columnsToDisplay = ['projectName', 'changed', 'settings', 'projectMenu'];
  currentUserName = '';

  constructor(private projectsService: ProjectsService) { }

  ngOnInit() {


    sessionStorage.setItem('currentUserName', 'Иван Иванов');
    sessionStorage.setItem('currentUserID', '300');
    this.currentUserName = sessionStorage.getItem('currentUserName');


    this.projectsService.getProjects ()
      .subscribe(project => { this.projectsArr = project; },
      error => console.error(error));

    this.projectsArr.forEach((element)=>{
      element.lastActivity = moment(element.lastActivity).fromNow();
    })
    // this.projectsArr = this.projectsService.getProjects() - метод возвращающий массив проетов. Отбор по юзеру не реализован
  }

}
