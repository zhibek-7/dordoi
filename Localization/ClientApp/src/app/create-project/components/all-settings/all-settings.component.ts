import { LocalizationProject } from './../../../models/database-entities/localizationProject.type';
import { Component, OnInit} from '@angular/core';


import { ProjectsService } from '../../../services/projects.service';



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
  currentProjectPublic = '1';
  currentProjectPrivate = '';
  currentProjectFileTrue = '';
  currentSkipUntranslStrTrue = '';
  currentProjectExportTrue = '';
  currentProjectNotificationTrue = '';

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

    this.currentProjectDescription = sessionStorage.getItem('ProjectDescription');
    console.log('ProjectDescription=' + sessionStorage.getItem('ProjectDescription'));
    console.log('ProjectDescriptionID=' + sessionStorage.getItem('ProjectDescriptionID'));

    this.currentProjectPublic = sessionStorage.getItem('currentProjectPublic');
    console.log('ProjectPublic=' + sessionStorage.getItem('ProjectPublic'));
    console.log('ProjectPublicID=' + sessionStorage.getItem('ProjectPublicID'));

    this.currentProjectPrivate = sessionStorage.getItem('currentProjectPrivate');
    console.log('ProjectPrivate=' + sessionStorage.getItem('ProjectPrivate'));
    console.log('ProjectPrivateID=' + sessionStorage.getItem('currentProjectPrivateID'));

    this.currentProjectFileTrue = sessionStorage.getItem('currentProjectFileTrue');
    console.log('ProjectFileTrue=' + sessionStorage.getItem('ProjectFileTrue'));
    console.log('ProjectFileTrueID=' + sessionStorage.getItem('ProjectFileTrueID'));

    this.currentSkipUntranslStrTrue = sessionStorage.getItem('currentSkipUntranslStrTrue');
    console.log('SkipUntranslStrTrue=' + sessionStorage.getItem('SkipUntranslStrTrue'));
    console.log('SkipUntranslStrTrueID=' + sessionStorage.getItem('SkipUntranslStrTrueID'));

    this.currentProjectExportTrue = sessionStorage.getItem('currentProjectExportTrue');
    console.log('ProjectExportTrue=' + sessionStorage.getItem('ProjectExportTrue'));
    console.log('ProjectExportTrueID=' + sessionStorage.getItem('ProjectExportTrueID'));

    this.currentProjectNotificationTrue = sessionStorage.getItem('currentProjectNotificationTrue');
    console.log('ProjectNotificationTrue=' + sessionStorage.getItem('ProjectNotificationTrue'));
    console.log('ProjectNotificationTrueID=' + sessionStorage.getItem('ProjectNotificationTrueID'));

  }


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
