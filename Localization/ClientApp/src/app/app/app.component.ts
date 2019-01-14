import { Component, OnInit } from '@angular/core';

import { ProjectsService } from '../services/projects.service';
import { LocalizationProject } from '../models/database-entities/localizationProject.type';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ProjectsService]
})
export class AppComponent implements OnInit{
  projects: LocalizationProject[];
  currentProject: LocalizationProject;
  name = '';
  constructor(private projectService: ProjectsService) {}

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


    //this.name = currentProject.name;
    console.log('ProjectName  currentProject.name ==' + currentProject.name);
    this.name =  this.projectService.currentProjectName;
    console.log('ProjectName projectsService.currentProjectName ==' + this.name);


    sessionStorage.setItem('ProjectName', currentProject.name);
    sessionStorage.setItem('ProjecID', currentProject.id.toString());



  }
}
