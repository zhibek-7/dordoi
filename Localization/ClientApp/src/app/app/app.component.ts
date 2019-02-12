import { Component, OnInit } from '@angular/core';

import { ProjectsService } from '../services/projects.service';
import { UserService } from '../services/user.service';
import { AuthenticationService } from '../services/authentication.service';

import { LocalizationProject } from '../models/database-entities/localizationProject.type';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ProjectsService, AuthenticationService]
})
export class AppComponent implements OnInit {
  projects: LocalizationProject[];
  currentProject: LocalizationProject;
  name = '';
  constructor(private projectService: ProjectsService,
              private authenticationService: AuthenticationService) { }

  ngOnInit() {
    this.projectService.getProjects()
      .subscribe(projects => { this.projects = projects; },
        error => console.error(error));
    console.log('app ngOnInit ---!!!!!!!!!');
    console.log('ProjectName ==' + sessionStorage.getItem('ProjectName'));
    this.name = sessionStorage.getItem('ProjectName');
  }

  createNewProject() {

    console.log('app createNewProject ---!!!!!!!!!');
  }

  getCurrentProject(currentProject: LocalizationProject) {
    this.currentProject = currentProject;


    //this.name_text = currentProject.name_text;
    console.log('ProjectName  currentProject.name_text ==' + currentProject.name_text);
    this.name = this.projectService.currentProjectName;
    console.log('ProjectName projectsService.currentProjectName ==' + this.name);


    sessionStorage.setItem('ProjectName', currentProject.name_text);
    sessionStorage.setItem('ProjecID', currentProject.id.toString());
  }

  logOut(){
    this.authenticationService.deleteToken();
  }

  // userAuthorized(): Observable<boolean> {

  //   var responseToForm: boolean;

  //   this.authenticationService.checkUserAuthorisation().pipe(map(res => console.log(res)));
  //     // .subscribe(
  //     //   response => {
  //     //     responseToForm = response;
  //     //     return responseToForm;
  //     //   }
  //     // );
  // }  
}
