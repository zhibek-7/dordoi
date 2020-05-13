import { Component, OnInit } from '@angular/core';

import { LocalizationProject } from "../../../models/database-entities/localizationProject.type";
import { LocalizationProjectsLocales } from "src/app/models/database-entities/localizationProjectLocales.type";

import { ProjectsService } from 'src/app/services/projects.service';
import { ProjectsLocalesService } from "src/app/services/projectsLocales.service";

//import { Guid } from 'guid-typescript';
import { Router } from "@angular/router";

@Component({
  selector: 'app-add-project',
  templateUrl: './add-project.component.html',
  styleUrls: ['./add-project.component.css']
})
export class AddProjectComponent implements OnInit {

  project: LocalizationProject;

  loaded: boolean = false;

  constructor(private projectsService: ProjectsService,
    private projectsLocalesService: ProjectsLocalesService,
    private router: Router) { }

  ngOnInit() {
    //this.project = new LocalizationProject(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
    this.project = new LocalizationProject(null, null, null, null, false, null, null, false, false, false, false, false, false, false, false, false);
    //this.project.date_Of_Creation = this.project.last_Activity = "2000-01-01T00:00:00";
    this.project.logo = null;



    this.loaded = true;
  }
  
  submit(editedProject: [LocalizationProject, LocalizationProjectsLocales[]]) {
    
    //console.log("LocalizationProject", editedProject[0]);
    //console.log("LocalizationProjectsLocales[]", editedProject[1]);


    this.projectsService.create(editedProject[0]).subscribe(
      projectId => {

        this.projectsService.currentProjectId = projectId;
        this.projectsService.currentProjectName = editedProject[0].name_text;
        
        this.projectsLocalesService.createProjectLocales(editedProject[1].map(t => new LocalizationProjectsLocales(projectId, t.iD_Locale))).subscribe(
          result => {
            this.router.navigate(["/Projects/" + projectId]);
          },
          error => console.error(error)
        );

      },
      error => console.error(error)
    );

  }

}
