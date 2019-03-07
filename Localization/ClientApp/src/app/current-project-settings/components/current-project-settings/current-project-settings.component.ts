import { Component, OnInit } from "@angular/core";
import { ProjectsService } from "../../../services/projects.service";

import * as moment from "moment";
moment.locale("ru");

@Component({
  selector: "app-current-project-settings",
  templateUrl: "./current-project-settings.component.html",
  styleUrls: ["./current-project-settings.component.css"]
})
export class CurrentProjectSettingsComponent implements OnInit {
 
  projectId: number;
  projectName: string;
  
  constructor(private projectService: ProjectsService) {}

  ngOnInit() {
    this.projectId = this.projectService.currentProjectId;
    this.projectName = this.projectService.currentProjectName;
  }
  
}
