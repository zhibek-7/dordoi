import { Component, OnInit } from "@angular/core";
import { ProjectsService } from "../../../services/projects.service";
import { ActivatedRoute } from "@angular/router";
import { Guid } from 'guid-typescript';

import * as moment from "moment";
moment.locale("ru");

@Component({
  selector: "app-current-project-settings",
  templateUrl: "./current-project-settings.component.html",
  styleUrls: ["./current-project-settings.component.css"]
})
export class CurrentProjectSettingsComponent implements OnInit {
 
  projectId: Guid;
  projectName: string;
  
  constructor(    private route: ActivatedRoute,
private projectService: ProjectsService) {}

  ngOnInit() {
    this.projectId = this.projectService.currentProjectId;
    this.projectName = this.projectService.currentProjectName;

     console.log(".CurrentProjectSettingsComponent=="+this.route.snapshot.params["projectId"]);
    if (this.route.snapshot.params["projectId"] != null) {
      sessionStorage.setItem("ProjectID", this.route.snapshot.params["projectId"]);
  }
  
}
}
