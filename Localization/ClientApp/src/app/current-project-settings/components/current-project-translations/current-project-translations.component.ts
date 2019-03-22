import { Component, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute, Params, Router } from "@angular/router";
import { LocalizationProject } from "../../../models/database-entities/localizationProject.type";
import { ProjectsService } from "../../../services/projects.service";

///-----
import { MatTableDataSource, MatSort } from "@angular/material";

import { LanguageService } from "src/app/services/languages.service";
import { UserService } from "src/app/services/user.service";
import { AuthenticationService } from "src/app/services/authentication.service";

import { Locale } from "src/app/models/database-entities/locale.type";
import { LocalizationProjectsLocalesDTO } from "src/app/models/DTO/localizationProjectsLocalesDTO";
import { Participant } from "src/app/models/Participants/participant.type";
import { Guid } from 'guid-typescript';
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
  projectId: Guid;
  //langList: Array<Locale>;

  constructor(
    private route: ActivatedRoute,
    private projectService: ProjectsService,
    private router: Router,
    private languagesService: LanguageService,
    private userService: UserService,
    private authenticationService: AuthenticationService
  ) {}

  // Загрузка файлов
  filesUpload(files): void {
    console.log(files);
  }

  ngOnInit() {
   console.log(
      ".CurrentProjectTranslationsComponent.snapshot=" +
        this.route.snapshot.params["projectId"]
    );
    if (this.route.snapshot.params["projectId"] != null) {
      this.projectId = this.route.snapshot.params["projectId"];
      sessionStorage.setItem("ProjectID", this.route.snapshot.params["projectId"]);
    }



    this.currentUserName = this.userService.currentUserName;

    this.projectId = this.projectService.currentProjectId;
    //var projectId = Number(sessionStorage.getItem("ProjectID"));
    console.log(
      "..CurrentProjectTranslationsComponent.projectId=" + this.projectId
    );
     this.getProject();

    /*this.languagesService.getByProjectId(this.projectId).subscribe(
      Languages => {
        this.langList = Languages;
      },
      error => console.error(error)
    );
    */

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

    this.authenticationService.setUserRoleAccordingToProject().subscribe(
      success => {
        this.authenticationService.refreshToken().subscribe();
      }
    );
  }

  ngAfterViewInit() {
    this.dataSourceForMainTab.sort = this.sortForMainTab;
  }

  getProject() {
    this.route.params.subscribe((params: Params) => {
      this.projectService
        .getProject(this.route.snapshot.params["projectId"])
        .subscribe(
          project => {
            this.currentProject = project;
            this.wasChanged = this.currentProject.date_Of_Creation;
            this.wasCreated = this.currentProject.last_Activity;
          },
          error => console.error(error)
        );
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
