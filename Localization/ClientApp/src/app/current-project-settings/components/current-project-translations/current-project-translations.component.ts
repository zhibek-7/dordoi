import { Component, OnInit, Input, ViewChild } from "@angular/core";
import { ActivatedRoute, Params, Router } from "@angular/router";
import { LocalizationProject } from "../../../models/database-entities/localizationProject.type";
import { ProjectsService } from "../../../services/projects.service";

///-----
import { MatTableDataSource, MatSort } from "@angular/material";

import { LanguageService } from "src/app/services/languages.service";
import { UserService } from "src/app/services/user.service";

import { Locale } from "src/app/models/database-entities/locale.type";
import { User } from "src/app/models/database-entities/user.type";
import { UserAction } from "src/app/models/database-entities/userAction.type";
import { LocalizationProjectsLocalesDTO } from "src/app/models/DTO/localizationProjectsLocalesDTO";
import { Participant } from "src/app/models/Participants/participant.type";

///---
import * as moment from "moment";
moment.locale("ru");

@Component({
  selector: "app-current-project-translations",
  templateUrl: "./current-project-translations.component.html",
  styleUrls: ["./current-project-translations.component.css"] //,
  //providers: [LanguageService, UserService]
})
export class CurrentProjectTranslationsComponent implements OnInit {
  currentProject: LocalizationProject;
  wasCreated: any;
  wasChanged: any;

  //#region для настройки таблицы (mat-table) отображаемой на вкладке "Главная"
  displayedColumnsForMainTab: string[] = [
    "flag",
    "locale_Name",
    "percent_Of_Translation",
    "percent_Of_Confirmed",
    "users",
    "download"
  ];
  public dataSourceForMainTab = new MatTableDataSource<
    LocalizationProjectsLocalesDTO
  >();
  @ViewChild(MatSort) sortForMainTab: MatSort;
  //#endregion

  //#region Для блока "Детали" на главной вкладке
  /** Менеджеры */
  participantsManager: Participant[];
  /** Владельцы */
  participantsOwner: Participant[];
  //#endregion

  currentUserName = "";
  projectId: number;
  langList: Array<Locale>;

  constructor(
    private route: ActivatedRoute,
    private projectService: ProjectsService,
    private router: Router,
    private languagesService: LanguageService,
    private userService: UserService
  ) {}

  // Загрузка файлов
  filesUpload(files): void {
    console.log(files);
  }

  ngOnInit() {
    this.getProject();

    //console.log("ProjectName=" + sessionStorage.getItem("ProjectName"));
    //console.log("ProjectID=" + sessionStorage.getItem("ProjectID"));
    //console.log("Projec=" + sessionStorage.getItem("Projec"));

    this.currentUserName = this.userService.currentUserName;
    //this.currentUserName = sessionStorage.getItem("currentUserName");

    this.projectId = this.projectService.currentProjectId;
    //var projectId = Number(sessionStorage.getItem("ProjectID"));

    this.languagesService.getByProjectId(this.projectId).subscribe(
      Languages => {
        this.langList = Languages;
      },
      error => console.error(error)
    );

    this.languagesService
      .getLocalesWithPercentByProjectId(this.projectId)
      .subscribe(
        localesWithPercent => {
          this.dataSourceForMainTab.data = localesWithPercent;
        },
        error => console.error(error)
      );

    this.projectService.getProjectWithDetails(this.projectId).subscribe(
      project => {
        this.currentProject = project;
        this.currentProject.date_Of_Creation = new Date(
          this.currentProject.date_Of_Creation
        );
        this.currentProject.last_Activity = new Date(
          this.currentProject.last_Activity
        );
      },
      error => console.error(error)
    );
  }

  ngAfterViewInit() {
    this.dataSourceForMainTab.sort = this.sortForMainTab;
  }

  getProject() {
    this.route.params.subscribe((params: Params) => {
      this.projectService
        .getProject(this.route.snapshot.params["id"])
        .subscribe(
          project => {
            this.currentProject = project;
            this.wasChanged = this.currentProject.date_Of_Creation;
            this.wasCreated = this.currentProject.last_Activity;
          },
          error => console.error(error)
        );
      console.log("snapshot=" + this.route.snapshot.params["id"]);
    });
  }

  openLanguageFiles(selectedLanguage: any) {
    this.router.navigate(
      [
        "/Translation_project/" +
          this.projectId +
          "/LanguageFiles/" +
          selectedLanguage.locale_Id
      ],
      {
        relativeTo: this.route
      }
    );
  }
}
