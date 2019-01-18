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
  currentProjectId = null;
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
    this.currentProjectId = sessionStorage.getItem('ProjecID');
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
    //let newProject: LocalizationProject = new LocalizationProject(this.settings_proj.get('pjName').value, this.settings_proj.get('pjName').value, this.settings_proj.get('pjDescription').value);// поменять на id реального пользователя, когда появится

   // this.projectsService.addProject(newProject);
  }
  editProject(Id: number): void {
    console.log("!!!=" + this.settings_proj.get('pjName').value);
    console.log("!!!=" + this.settings_proj.get('pjDescription').value);
    console.log("!!!=" + this.settings_proj.get('pjPublic').value);
    console.log("!!!=" + this.settings_proj.get('pjPrivate').value);
    console.log("!!!=" + this.settings_proj.get('pjFileTrue').value);
    console.log("!!!=" + this.settings_proj.get('pjSkipUntranslStrTrue').value);
    console.log("!!!=" + this.settings_proj.get('pjExportTrue').value);
    let newProject: LocalizationProject = new LocalizationProject(
      this.currentProjectId,
      this.settings_proj.get('pjName').value,
      this.settings_proj.get('pjDescription').value,
     // this.settings_proj.get('pjDescription').value,
      this.settings_proj.get('pjPublic').value,//visibility
    //  this.settings_proj.get('pjDescription').value,//date dateOfCreation
     // this.settings_proj.get('pjDescription').value,//date lastActivity
      this.settings_proj.get('pjFileTrue').value,//ableToDownload
      this.settings_proj.get('pjSkipUntranslStrTrue').value,//ableToLeftErrors
      this.settings_proj.get('pjExportTrue').value,
      this.settings_proj.get('pjPublic').value,
      this.settings_proj.get('pjExportTrue').value,
      this.settings_proj.get('pjExportTrue').value,
      this.settings_proj.get('pjExportTrue').value,
      this.settings_proj.get('pjExportTrue').value,
      this.settings_proj.get('pjExportTrue').value);// поменять на id реального пользователя, когда появится
    Id = this.currentProjectId;
    this.projectsService.updateProject(Id, newProject);
  }


  /*
visibility: boolean,
    dateOfCreation: Date,
    lastActivity: Date,
    ableToDownload: boolean,
    ableToLeftErrors: boolean,
    defaultString: boolean,
    notifyNew: boolean,
    notifyFinish: boolean,
    notifyConfirm: boolean,
    notifynewcomment: boolean,
    export_only_approved_translations: boolean,
    original_if_string_is_not_translated: boolean
*/
}

