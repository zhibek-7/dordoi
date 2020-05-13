import { Component, OnInit } from '@angular/core';

import { LocalizationProject } from "../../../models/database-entities/localizationProject.type";
import { LocalizationProjectsLocales } from "src/app/models/database-entities/localizationProjectLocales.type";

import { ProjectsService } from 'src/app/services/projects.service';
import { ProjectsLocalesService } from "src/app/services/projectsLocales.service";

import { Router } from "@angular/router";

@Component({
  selector: 'app-edit-project',
  templateUrl: './edit-project.component.html',
  styleUrls: ['./edit-project.component.css']
})
export class EditProjectComponent implements OnInit {

  project: LocalizationProject;

  loaded: boolean = false;

  constructor(private projectsService: ProjectsService,
    private projectsLocalesService: ProjectsLocalesService,
    private router: Router) { }

  ngOnInit() {
    this.projectsService.getProject(this.projectsService.currentProjectId).subscribe(
      project => {
        this.project = project;
        this.loaded = true;
      },
      error => console.error(error)
    );
  }

  submit(editedProject: [LocalizationProject, LocalizationProjectsLocales[]]) {
    this.projectsService.update(editedProject[0]).subscribe(
      result => { },
      error => console.error(error)
    );

    this.projectsLocalesService.editProjectLocales(editedProject[0].id, editedProject[1]).subscribe(
      result => { },
      error => console.error(error)
    );
  }
  
  delete(confirm: boolean) {
    if (confirm)
    {
      this.projectsService.delete(this.project.id).subscribe(
        result => {
          this.router.navigate(["/Profile"]);
        },
        error => console.error(error)
      );
    }
  }

}
