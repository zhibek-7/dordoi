import { Component, OnInit } from '@angular/core';
import { ProjectsService } from '../../../services/projects.service';
import { Router } from "@angular/router";
@Component({
  selector: 'app-delete-project',
  templateUrl: './delete-project.component.html',
  styleUrls: ['./delete-project.component.css']
})
export class DeleteProjectComponent implements OnInit {
  currentProjectId = null;
  constructor(private router: Router, private projectService: ProjectsService) { }

  ngOnInit() {
    this.currentProjectId = sessionStorage.getItem('ProjecID');
    console.log('ProjecID=' + sessionStorage.getItem('ProjecID'));
  }

  deleteProject(): void {
    let Id = this.currentProjectId;
    //удаляет проекта

    //удаляет языки с проекта
    this.projectService.deleteProjectLocalesById(Id).subscribe(loc => {

    });
    this.projectService.deleteProject(Id);


    this.router.navigate(["/Projects/"]);
  }
}
