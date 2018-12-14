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

  constructor(private route: ActivatedRoute, private projectService: ProjectsService) { }

  ngOnInit() {
    this.route.params
      .subscribe((params: Params) => {
        this.projectService.getProject(this.route.snapshot.params['id'])
          .subscribe(project => {
              this.currentProject = project;
            },
            error => console.error(error));
        console.log(this.route.snapshot.params['id']);
      });
  }

}
