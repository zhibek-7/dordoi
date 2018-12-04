import { Component, OnInit } from '@angular/core';
import { ProjectsService } from '../services/projects.service'
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

  constructor(private projectsService: ProjectsService) { }

  ngOnInit() {
    this.projectsArr = this.projectsService.projects;
    this.projectsArr.forEach((element)=>{
      element.changed = moment(element.changed).fromNow();
      element.created = moment(element.created).fromNow();
    })
    // this.projectsArr = this.projectsService.getProjects() - метод возвращающий массив проетов. Отбор по юзеру не реализован
  }

}
