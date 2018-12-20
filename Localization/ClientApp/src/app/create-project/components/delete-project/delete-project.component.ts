import { Component, OnInit } from '@angular/core';
import { ProjectsService } from '../../../services/projects.service';

@Component({
  selector: 'app-delete-project',
  templateUrl: './delete-project.component.html',
  styleUrls: ['./delete-project.component.css']
})
export class DeleteProjectComponent implements OnInit {

  constructor(private projectService: ProjectsService) { }
  ngOnInit() {
  }
  deleteProject(Id: number): void {
    this.projectService.deleteProject(Id).subscribe(response => console.log(response));
  }
}
