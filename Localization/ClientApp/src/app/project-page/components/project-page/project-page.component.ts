import { Component, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { LocalizationProject } from "src/app/models/database-entities/localizationProject.type";
import { ProjectsService } from "src/app/services/projects.service";
import { MatTableDataSource, MatSort } from "@angular/material";

import { LanguageService } from "src/app/services/languages.service";
import { UserService } from "src/app/services/user.service";
import { UserActionsService } from "src/app/services/userActions.service";
import { ParticipantsService } from "src/app/services/participants.service";
import { AuthenticationService } from "src/app/services/authentication.service";

import { Locale } from "src/app/models/database-entities/locale.type";
import { User } from "src/app/models/database-entities/user.type";
import { UserAction } from "src/app/models/database-entities/userAction.type";
import { LocalizationProjectsLocalesDTO } from "src/app/models/DTO/localizationProjectsLocalesDTO";
import { Participant } from "src/app/models/Participants/participant.type";

@Component({
  selector: "app-project-page",
  templateUrl: "./project-page.component.html",
  styleUrls: ["./project-page.component.css"],
  providers: [
    LanguageService,
    UserService,
    UserActionsService,
    ParticipantsService
  ]
})
export class ProjectPageComponent implements OnInit {
  currentProject: LocalizationProject;

  langList: Array<Locale>;
  userList = new Array<User>();
  userActionsList: Array<UserAction>;
  userActionsDataSource = new MatTableDataSource(this.userActionsList);

  panelOpenState = false;
  selectedUser = "none";
  selectedLang = "none";

  filtredUsers = new Array<User>();

  displayedColumns: string[] = ["variant", "file", "language", "time"];

  currentUserName = "";

  //Для блока "Детали" на главной вкладке
  /** Менеджеры */
  participantsManager: Participant[];
  /** Владельцы */
  participantsOwner: Participant[];

  //#region для настройки таблицы (mat-table) отображаемой на вкладке "Главная"
  displayedColumnsForMainTab: string[] = [
    "flag",
    "locale_Name",
    "percent_Of_Translation",
    "percent_Of_Confirmed"
  ];
  private dataSourceForMainTab = new MatTableDataSource<
    LocalizationProjectsLocalesDTO
  >();
  @ViewChild(MatSort) sortForMainTab: MatSort;
  //#endregion

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectsService,
    private languagesService: LanguageService,
    private userService: UserService,
    private userActionsService: UserActionsService,
    private participantsService: ParticipantsService,
    private authenticationService: AuthenticationService
  ) {}

  ngOnInit() {
    this.currentUserName = this.userService.currentUserName;
    //this.currentUserName = sessionStorage.getItem("currentUserName");

    var projectId = this.projectService.currentProjectId;
    //var projectId = Number(sessionStorage.getItem("ProjecID"));

    this.authenticationService.setUserRoleAccordingToProject().subscribe(
      success => {
        this.authenticationService.refreshToken().subscribe();
      }
    );

    this.languagesService.getByProjectId(projectId).subscribe(
      Languages => {
        this.langList = Languages;
      },
      error => console.error(error)
    );

    this.languagesService.getLocalesWithPercentByProjectId(projectId).subscribe(
      localesWithPercent => {
        this.dataSourceForMainTab.data = localesWithPercent;
      },
      error => console.error(error)
    );

    this.participantsService
      .getParticipantsByProjectId(
        projectId,
        null,
        null,
        null,
        null,
        null,
        ["user_Name"],
        true,
        ["owner", "manager"]
      )
      .subscribe(
        response => {
          this.participantsManager = response.body.filter(
            t => t.role_Short.toString() == "manager"
          );
          this.participantsOwner = response.body.filter(
            t => t.role_Short == "owner"
          );
        },
        error => console.error(error)
      );

    this.projectService.getProjectWithDetails(projectId).subscribe(
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

    this.userService.getProjectParticipantList(projectId).subscribe(
      Users => {
        this.userList = Users;
      },
      error => console.error(error)
    );

    this.userActionsService.getActionsList().subscribe(
      actions => {
        this.userActionsList = actions;
        this.userActionsDataSource = new MatTableDataSource(this.userActionsList);
      },
      error => console.error(error)
    );

    this.filtredUsers = this.userList;
  }

  ngAfterViewInit() {
    this.dataSourceForMainTab.sort = this.sortForMainTab;
  }

  applyFilterUsers(filterValue: string) {
    if (filterValue === "none") {
      this.filtredUsers = this.userList;
    } else {
      this.filtredUsers = this.userList.filter(function(i) {
        return filtredArr(i.id);
      });
    }

    function filtredArr(id) {
      return id === filterValue;
    }
  }

  applyFilterLang(filterValue) {
    let currentLang = filterValue.source.triggerValue;

    if (currentLang === "Все языки") {
      this.userActionsDataSource = new MatTableDataSource(this.userActionsList);
    } else {
      const filtredLangArr = this.userActionsList.filter(function(i) {
        return filtredArr(i.locale_name);
      });
      this.userActionsDataSource = new MatTableDataSource(filtredLangArr);
    }

    function filtredArr(language) {
      return language === currentLang;
    }
  }

  openLanguageFiles(selectedLanguage: any) {
    this.router.navigate(["LanguageFiles/" + selectedLanguage.locale_Id], {
      relativeTo: this.route
    });
  }
}
