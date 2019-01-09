import { LocalizationProject } from './../../../models/database-entities/localizationProject.type';
import { Component, OnInit} from '@angular/core';
import { ProjectsService } from '../../../services/projects.service';

import { Project } from '../../../models/Project/Project';

import { FormControl} from '@angular/forms';



@Component({
  selector: 'app-all-settings',
  templateUrl: './all-settings.component.html',
  styleUrls: ['./all-settings.component.css']
})
export class AllSettingsComponent implements OnInit {
  args = 'ascending';
  reverse = false;
  constructor(private projectsService: ProjectsService) { }
  currentProjectName = '';
  currentProjectDescription = '';
  currentProjectPublic = true;
  currentProjectPrivate = false;
  currentProjectFileTrue = false;
  currentSkipUntranslStrTrue = false;
  currentProjectExportTrue = false;
  currentProjectNotificationTrue = false;
  currentProjectdateOfCreation =Date.now();
  currentProjectID_SourceLocale = 1;
  currentProjectlastActivity = Date.now();
  currentProjectlogo = '';
  currentProjectid = 1;
  currentProjecturl = '';

  pjName = new FormControl();
  pjDescription = new FormControl();
  pjPublic = new FormControl();
  pjPrivate = new FormControl();
  pjFileTrue = new FormControl();
  pjSkipUntranslStrTrue = new FormControl();
  pjExportTrue = new FormControl();
  pjNotificationTrue = new FormControl();


  ngOnInit() {
    this.currentProjectName = sessionStorage.getItem('ProjectName');
    console.log('ProjectName=' + sessionStorage.getItem('ProjectName'));
    console.log('ProjecID=' + sessionStorage.getItem('ProjecID'));

 

  }

  name?: string;
  ableToDownload?: boolean;
  ableToLeftErrors?: boolean;
  dateOfCreation?: Date;
  defaultString?: string;
  description?: string;
  ID_SourceLocale?: number;
  lastActivity?: Date;
  logo?: string;
  notifyConfirm?: boolean;
  notifyFinish?: boolean;
  notifyNew?: boolean;
  visibility?: boolean;
  id?: number;
  url?: string;

  addProject() {
    console.log("!!!=" + this.pjName.value);
    console.log("!!!=" + this.pjDescription.value);
    console.log("!!!=" + this.pjPublic.value);
    console.log("!!!=" + this.pjPrivate.value);
    console.log("!!!=" + this.pjFileTrue.value);
    console.log("!!!=" + this.pjSkipUntranslStrTrue.value);
    console.log("!!!=" + this.pjExportTrue.value);
    console.log("!!!=" + this.pjNotificationTrue.value);
    let newProject: LocalizationProject = new LocalizationProject(this.pjName.value, this.pjName.value, this.pjDescription.value);// поменять на id реального пользователя, когда появится


    
    this.projectsService.addProject(newProject);

  }


}
