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
  langList;

  constructor(private route: ActivatedRoute, private projectService: ProjectsService) { }

  ngOnInit() {
    console.log('ProjectName=' + sessionStorage.getItem('ProjectName'));
    console.log('ProjecID=' + sessionStorage.getItem('ProjecID'));

    this.getProject();
    this.langList =  [
      {name: 'Французский', icon: '../../assets/images/11.png'},
      {name: 'Мандинго', icon: '../../assets/images/22.png'},
      {name: 'Испанский', icon: '../../assets/images/333.png'},
    ];
  }

  getProject() {

    this.projectId = Number(sessionStorage.getItem('ProjecID'));

    this.route.params.subscribe((params: Params) => {
      this.projectService
        .getProject(this.projectId)
        .subscribe(
          project => {
            this.currentProject = project;
            console.log(project)
            // this.langList = this.currentProject.lang - где-то здесь надо получить с сервера
            // еще и языки проекта, которые как я понимаю будут одним из свойсвт проекта
          },
          error => console.error(error)
        );
      console.log('snapshot=' + this.route.snapshot.params['id']);
    });
  }

}
