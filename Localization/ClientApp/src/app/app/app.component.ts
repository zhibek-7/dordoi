import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { ProjectsService } from '../services/projects.service';
import { AuthenticationService } from '../services/authentication.service';

import { LocalizationProject } from '../models/database-entities/localizationProject.type';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ProjectsService]
})
export class AppComponent implements OnInit {

  projects: LocalizationProject[];
  currentProject: LocalizationProject;
  name = '';

  userAuthorized: boolean = false;

  constructor(private projectService: ProjectsService,
              private authenticationService: AuthenticationService,
              private router: Router) { }

  ngOnInit() {    

    // this.getProjects();
    console.log('app ngOnInit ---!!!!!!!!!');
    console.log('ProjectName ==' + sessionStorage.getItem('ProjectName'));
    this.name = sessionStorage.getItem('ProjectName');
  }

  createNewProject() {
    console.log('app createNewProject ---!!!!!!!!!');
  }

  getProjects() {

    this.projectService.getProjects()
    .subscribe(projects => { this.projects = projects; },
      error => console.error(error));
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

  async checkAuthorization() {
    this.userAuthorized = await this.authenticationService.checkUserAuthorisation();  
  }
 
}
