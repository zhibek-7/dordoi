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
  constructor(private projectService: ProjectsService) {}

  ngOnInit() {
    this.projectService.getProjects()
      .subscribe(projects => { this.projects = projects; },
        error => console.error(error));
    console.log('app ngOnInit ---!!!!!!!!!');
  }

  createNewProject() {

    console.log('app createNewProject ---!!!!!!!!!');
  }

  getCurrentProject(currentProject: LocalizationProject) {
    this.currentProject = currentProject;

    sessionStorage.setItem('ProjectName', currentProject.name);
    sessionStorage.setItem('ProjecID', currentProject.id.toString());

  }
}
