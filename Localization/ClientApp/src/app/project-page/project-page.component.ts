import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { LocalizationProject } from '../models/database-entities/localizationProject.type';
import { ProjectsService } from '../services/projects.service';
import { MatTableDataSource } from '@angular/material';

import { LanguageService } from '../services/languages.service';
import { UserService } from '../services/user.service';
import { WorkTypeService } from '../services/workType.service';
import { UserActionsService } from '../services/userActions.service';


import { Locale } from '../models/database-entities/locale.type';
import { User } from '../models/database-entities/user.type';
import { WorkType } from "../models/database-entities/workType.type";
import { UserAction } from '../models/database-entities/userAction.type';

@Component({
  selector: 'app-project-page',
  templateUrl: './project-page.component.html',
  styleUrls: ['./project-page.component.css'],
  providers: [LanguageService, UserService, WorkTypeService, UserActionsService ]
})

export class ProjectPageComponent implements OnInit {

  currentProject: LocalizationProject;

  langList: Array<Locale>;
  userList = new Array<User>();
  workTypeList: Array<WorkType>;
  userActionsList: Array<UserAction>;
  dataSource = new MatTableDataSource(this.userActionsList);

  panelOpenState = false;
  selectedUser = 'none';
  selectedLang = 'none';
  selectedWorkType = 'none';
  
  filtredUsers = [];

  displayedColumns: string[] = ['variant', 'file', 'language', 'time'];

  currentUserName = '';

  constructor(
    private route: ActivatedRoute,
    private projectService: ProjectsService,
    private languagesService: LanguageService,
    private userService: UserService,
    private workTypeService: WorkTypeService, 
    private userActionsService: UserActionsService,
  ) { }

  ngOnInit() {
    this.currentUserName = sessionStorage.getItem('currentUserName');

    this.workTypeService.getWorkTypes()
      .subscribe(workTypes => { this.workTypeList = workTypes; },
        error => console.error(error));

    var projectId = Number(sessionStorage.getItem('ProjecID'));

    this.languagesService.getByProjectId(projectId)
      .subscribe(Languages => { this.langList = Languages; },
        error => console.error(error));

    this.projectService.getProject(projectId)
      .subscribe(
        project => { this.currentProject = project; },
        error => console.error(error));

    this.userService.getProjectParticipantList(projectId)
      .subscribe(
        Users => { this.userList = Users; },
        error => console.error(error));

    this.userActionsService.getActionsList()
      .subscribe(actions => {
        this.userActionsList = actions;
        this.dataSource = new MatTableDataSource(this.userActionsList);
        },
        error => console.error(error));

    this.filtredUsers = this.userList;
  }

  applyFilterUsers(filterValue: string) {
    if (filterValue === 'none') {
      this.filtredUsers = this.userList;
    } else {
      this.filtredUsers = this.userList.filter(function(i) {
        return filtredArr(i.id);
      });
    }

    function filtredArr(id) {
      return id === filterValue;
    }
  }

  applyFilterLang(filterValue) {
    let currentLang = filterValue.source.triggerValue;

    if (currentLang === 'Все языки') {
      this.dataSource = new MatTableDataSource(this.userActionsList);
    }
    else {
      const filtredLangArr = this.userActionsList.filter(function(i) {
        return filtredArr(i.Locale);
      });
      this.dataSource = new MatTableDataSource(filtredLangArr);
    }

    function filtredArr(language) {
      return language === currentLang;
    }
  }

  applyActionsFilter()
  {
    console.log(this.selectedWorkType);
    console.log(this.selectedUser);
    console.log(this.selectedLang);
  }
}
