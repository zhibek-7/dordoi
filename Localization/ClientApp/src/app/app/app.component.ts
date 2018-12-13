import { Component, OnInit } from '@angular/core';

import { ProjectsService } from '../services/projects.service';
import { Project } from '../models/Project';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ProjectsService]
})
export class AppComponent implements OnInit{
  projects: Project[];
  currentProject: Project;
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

  getCurrentProject(currentProject: Project) {
    this.currentProject = currentProject;
  }
}
