import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Project } from '../models/Project';
import { ProjectsService } from '../services/projects.service';

@Component({
  selector: 'app-project-page',
  templateUrl: './project-page.component.html',
  styleUrls: ['./project-page.component.css']
})
export class ProjectPageComponent implements OnInit {
  currentProject: Project;
  name: string;
  projectId: number;

  constructor(private route: ActivatedRoute, private projectService: ProjectsService) { }

  ngOnInit() {
    console.log('ProjectName=' + sessionStorage.getItem('ProjectName'));
    console.log('ProjecID=' + sessionStorage.getItem('ProjecID'));

    this.getProject();
  }

  getProject() {

    this.projectId = Number(sessionStorage.getItem('ProjecID'));

    this.route.params.subscribe((params: Params) => {
      this.projectService
        .getProject(this.projectId)
        .subscribe(
          project => {
            this.currentProject = project;
          },
          error => console.error(error)
        );
      console.log('snapshot=' + this.route.snapshot.params['id']);
    });
  }

}
