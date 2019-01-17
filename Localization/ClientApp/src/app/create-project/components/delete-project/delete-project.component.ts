import { Component, OnInit } from '@angular/core';
import { ProjectsService } from '../../../services/projects.service';

@Component({
  selector: 'app-delete-project',
  templateUrl: './delete-project.component.html',
  styleUrls: ['./delete-project.component.css']
})
export class DeleteProjectComponent implements OnInit {
  currentProjectId = null;
  constructor(private projectService: ProjectsService) { }

  ngOnInit() {
    this.currentProjectId = sessionStorage.getItem('ProjecID');
    console.log('ProjecID=' + sessionStorage.getItem('ProjecID'));
  }

  deleteProject(Id: number): void {
    Id = this.currentProjectId;
    this.projectService.deleteProject(Id).subscribe(response => console.log(response));
  }
}
