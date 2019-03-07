import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Location } from "@angular/common";
import { LocalizationProject } from "../models/database-entities/localizationProject.type";

import { ProjectsService } from "../services/projects.service";

@Component({
  selector: "app-reports",
  templateUrl: "./Reports.component.html",
  styleUrls: ["./Reports.component.css"]
})
export class ReportsComponent implements OnInit {
  public projectId: number;
  public project: LocalizationProject;

  showProjectStatus: boolean = false;
  showCostSumm: boolean = false;
  showTranslationCost: boolean = false;
  showTranslatedWords: boolean = false;
  showFoulsTranslation: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private projectsService: ProjectsService
  ) {}

  ngOnInit() {
    console.log("--Reports.component ngOnInit-");
    console.log(
      "--Reports.component ngOnInit-1 ==" + this.route.snapshot.params["id"]
    );
    console.log(
      "--Reports.component ngOnInit2 = ==-" +
        this.projectsService.currentProjectId
    );

    var id = this.projectsService.currentProjectId; // this.route.snapshot.params["id"];
    this.projectsService.getProject(id).subscribe(
      project => {
        this.project = project;
        this.projectId = project.id;
      },
      error => console.error(error)
    );
  }

  public showReport(type: Number) {
    console.log("--Reports.component showReport-");
    this.showProjectStatus = type === 1;
    this.showCostSumm = type === 2;
    this.showTranslationCost = type === 3;
    this.showTranslatedWords = type === 4;
    this.showFoulsTranslation = type === 5;
  }

  goBack(): void {
    this.location.back();
  }
}
