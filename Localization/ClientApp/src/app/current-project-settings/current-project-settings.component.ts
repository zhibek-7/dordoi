import { Component, OnInit, Input } from "@angular/core";
import { ActivatedRoute, Params } from "@angular/router";
import { LocalizationProject } from '../models/database-entities/localizationProject.type';
import { ProjectsService } from "../services/projects.service";
import * as moment from "moment";
moment.locale("ru");

@Component({
  selector: "app-current-project-settings",
  templateUrl: "./current-project-settings.component.html",
  styleUrls: ["./current-project-settings.component.css"]
})
export class CurrentProjectSettingsComponent implements OnInit {
  currentProject: LocalizationProject;
  wasCreated: any;
  wasChanged: any;

  // Загрузка файлов
  filesUpload(files): void {
    console.log(files);
  }

  constructor(
    private route: ActivatedRoute,
    private projectService: ProjectsService
  ) {}

  ngOnInit() {
    this.getProject();

    console.log('ProjectName=' + sessionStorage.getItem('ProjectName'));
    console.log('ProjecID=' + sessionStorage.getItem('ProjecID'));
    console.log('Projec=' + sessionStorage.getItem('Projec'));


  }

  getProject() {
    this.route.params.subscribe((params: Params) => {
      this.projectService
        .getProject(this.route.snapshot.params["id"])
        .subscribe(
          project => {
            this.currentProject = project;
            this.wasChanged = this.currentProject.dateOfCreation;
            this.wasCreated = this.currentProject.lastActivity;
          },
          error => console.error(error)
        );
      console.log('snapshot='+this.route.snapshot.params["id"]);
    });
  }
}
