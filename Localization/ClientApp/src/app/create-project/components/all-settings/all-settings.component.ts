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

    this.currentProjectDescription = sessionStorage.getItem('ProjectDescription');
    console.log('ProjectDescription=' + sessionStorage.getItem('ProjectDescription'));
    console.log('ProjectDescriptionID=' + sessionStorage.getItem('ProjectDescriptionID'));

    //this.currentProjectPublic = sessionStorage.getItem('currentProjectPublic');
    //console.log('ProjectPublic=' + sessionStorage.getItem('ProjectPublic'));
    //console.log('ProjectPublicID=' + sessionStorage.getItem('ProjectPublicID'));

    //this.currentProjectPrivate = sessionStorage.getItem('currentProjectPrivate');
    //console.log('ProjectPrivate=' + sessionStorage.getItem('ProjectPrivate'));
    //console.log('ProjectPrivateID=' + sessionStorage.getItem('currentProjectPrivateID'));

    //this.currentProjectFileTrue = sessionStorage.getItem('currentProjectFileTrue');
    //console.log('ProjectFileTrue=' + sessionStorage.getItem('ProjectFileTrue'));
    //console.log('ProjectFileTrueID=' + sessionStorage.getItem('ProjectFileTrueID'));

    //this.currentSkipUntranslStrTrue = sessionStorage.getItem('currentSkipUntranslStrTrue');
    //console.log('SkipUntranslStrTrue=' + sessionStorage.getItem('SkipUntranslStrTrue'));
    //console.log('SkipUntranslStrTrueID=' + sessionStorage.getItem('SkipUntranslStrTrueID'));

    //this.currentProjectExportTrue = sessionStorage.getItem('currentProjectExportTrue');
    //console.log('ProjectExportTrue=' + sessionStorage.getItem('ProjectExportTrue'));
    //console.log('ProjectExportTrueID=' + sessionStorage.getItem('ProjectExportTrueID'));

    //this.currentProjectNotificationTrue = sessionStorage.getItem('currentProjectNotificationTrue');
    //console.log('ProjectNotificationTrue=' + sessionStorage.getItem('ProjectNotificationTrue'));
    //console.log('ProjectNotificationTrueID=' + sessionStorage.getItem('ProjectNotificationTrueID'));

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

    let newProject: Project = new Project(
      this.pjName.value,
      this.pjPublic.value,
      this.pjPrivate.value,
      this.pjFileTrue.value,
      this.pjSkipUntranslStrTrue.value,
      this.pjExportTrue.value,
      this.pjNotificationTrue.value,
    );


    // поменять на id реального пользователя, когда появится
    this.projectsService.addProject(newProject);

  }


}
