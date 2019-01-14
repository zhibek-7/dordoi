import { LocalizationProject } from './../../../models/database-entities/localizationProject.type';
import { Component, OnInit} from '@angular/core';
import { ProjectsService } from '../../../services/projects.service';
import { FormControl, FormGroup } from '@angular/forms';


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
 

  settings_proj = new FormGroup({
    pjName : new FormControl(),
    pjDescription : new FormControl(),
    pjPublic : new FormControl(),
    pjPrivate : new FormControl(),
    pjFileTrue : new FormControl(),
    pjSkipUntranslStrTrue : new FormControl(),
    pjExportTrue : new FormControl(),
    pjNotificationTrue : new FormControl()
  });


   

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
    console.log("!!!=" + this.settings_proj.get('pjName').value);
    console.log("!!!=" + this.settings_proj.get('pjDescription').value);
    console.log("!!!=" + this.settings_proj.get('pjPublic').value);
    //console.log("!!!=" + this.settings_proj.get('pjPrivate').value);
    console.log("!!!=" + this.settings_proj.get('pjFileTrue').value);
    console.log("!!!=" + this.settings_proj.get('pjSkipUntranslStrTrue').value);
    console.log("!!!=" + this.settings_proj.get('pjExportTrue').value);
    let newProject: LocalizationProject = new LocalizationProject(this.settings_proj.get('pjName').value, this.settings_proj.get('pjName').value, this.settings_proj.get('pjDescription').value);// поменять на id реального пользователя, когда появится
    this.projectsService.addProject(newProject);
  }


}

