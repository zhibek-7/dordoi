import { Component, OnInit } from '@angular/core';

import { LocalizationProject } from "../../../models/database-entities/localizationProject.type";
import { LocalizationProjectsLocales } from "src/app/models/database-entities/localizationProjectLocales.type";

import { ProjectsService } from 'src/app/services/projects.service';
import { ProjectsLocalesService } from "src/app/services/projectsLocales.service";

@Component({
  selector: 'app-edit-project',
  templateUrl: './edit-project.component.html',
  styleUrls: ['./edit-project.component.css']
})
export class EditProjectComponent implements OnInit {

  project: LocalizationProject;

  loaded: boolean = false;

  constructor(private projectsService: ProjectsService,
    private projectsLocalesService: ProjectsLocalesService) { }

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

    console.log("LocalizationProject", editedProject[0]);
    console.log("LocalizationProjectsLocales[]", editedProject[1]);

    this.projectsService.update(editedProject[0]).subscribe(
      result => { },
      error => console.error(error)
    );

    this.projectsLocalesService.editProjectLocales(editedProject[0].id, editedProject[1]).subscribe(
      result => { },
      error => console.error(error)
    );
  }
}
