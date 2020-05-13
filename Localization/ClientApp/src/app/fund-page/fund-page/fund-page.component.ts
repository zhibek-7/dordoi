import { Component, OnInit, ViewChild } from '@angular/core';
import { LanguageService } from '../../services/languages.service';
import { UserService } from '../../services/user.service';
import { ParticipantsService } from '../../services/participants.service';
import { UserActionsService } from '../../services/userActions.service';
import { fund } from "../../models/database-entities/fund.type";
import { User } from '../../models/database-entities/user.type';
import { UserAction } from '../../models/database-entities/userAction.type';
import { MatTableDataSource, MatSort } from '@angular/material';
import { Participant } from '../../models/Participants/participant.type';
import { LocalizationProjectsLocalesDTO } from '../../models/DTO/localizationProjectsLocalesDTO';
import { ActivatedRoute, Router } from '@angular/router';
import { FundService } from '../../services/fund.service';
import { AuthenticationService } from '../../services/authentication.service';
@Component({
  selector: 'app-fund-page',
  templateUrl: './fund-page.component.html',
  styleUrls: ['./fund-page.component.css'],
  providers: [
    LanguageService,
    UserService,
    UserActionsService,
    ParticipantsService
  ]
})
export class FundPageComponent implements OnInit {

  currentFund: fund;

  //langList: Array<Locale>;
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
    //    "flag",
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
    private fundService: FundService,
    private languagesService: LanguageService,
    private userService: UserService,
    private participantsService: ParticipantsService,
    private authenticationService: AuthenticationService
  ) { }

  ngOnInit() {
    this.currentUserName = this.userService.currentUserName;
    //this.currentUserName = sessionStorage.getItem("currentUserName");

    var fundId = this.fundService.currentFundId;
    //var projectId = Number(sessionStorage.getItem("ProjectID"));

    this.authenticationService.setUserRoleAccordingToProject().subscribe(
      success => {
        this.authenticationService.refreshToken().subscribe();
      }
    );

    //this.languagesService.getByProjectId(projectId).subscribe(
    //  Languages => {
    //    this.langList = Languages;
    //  },
    //  error => console.error(error)
    //);

    //this.languagesService.getLocalesWithPercentByProjectId(projectId).subscribe(
    //  localesWithPercent => {
    //    this.dataSourceForMainTab.data = localesWithPercent;
    //  },
    //  error => console.error(error)
    //);

    this.participantsService
      .getParticipantsByProjectId(
        fundId,
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

    this.fundService.getFundWithDetails(fundId).subscribe(
      fund => {
        this.currentFund = fund;
        this.currentFund.date_time_added = new Date(
          this.currentFund.date_time_added
        );
        //this.currentFund. = new Date(
        //  this.currentFund.last_Activity
        //);
      },
      error => console.error(error)
    );

    this.userService.getProjectParticipantList(fundId).subscribe(
      Users => {
        this.userList = Users;
      },
      error => console.error(error)
    );
    /*
        this.userActionsService.getActionsList(this.projectService.currentProjectId).subscribe(
          actions => {
            this.userActionsList = actions;
            this.userActionsDataSource = new MatTableDataSource(this.userActionsList);
          },
          error => console.error(error)
        );
      */
    this.filtredUsers = this.userList;
  }

  ngAfterViewInit() {
    this.dataSourceForMainTab.sort = this.sortForMainTab;
  }

  applyFilterUsers(filterValue: string) {
    if (filterValue === "none") {
      this.filtredUsers = this.userList;
    } else {
      this.filtredUsers = this.userList.filter(function (i) {
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
      const filtredLangArr = this.userActionsList.filter(function (i) {
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
