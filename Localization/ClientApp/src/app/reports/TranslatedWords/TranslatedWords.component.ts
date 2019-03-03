import { Component, OnInit, Input } from "@angular/core";

import { ReportService } from "../../services/reports.service";
import { LanguageService } from "../../services/languages.service";
import { UserService } from "../../services/user.service";
import { FileService } from "../../services/file.service";
import { ProjectsService } from "../../services/projects.service";

import { TranslatedWordsReportRow } from "../../models/Reports/TranslatedWordsReportRow";
import { Locale } from "../../models/database-entities/locale.type";
import { File } from "../../models/database-entities/file.type";
import { User } from "../../models/database-entities/user.type";
import { LocalizationProject } from "../../models/database-entities/localizationProject.type";

import { MatTableDataSource } from "@angular/material";

import { Moment } from "moment";
import * as moment from "moment";

@Component({
  selector: "translated-words-report",
  templateUrl: "./TranslatedWords.component.html",
  styleUrls: ["./TranslatedWords.component.css"]
})
export class TranslatedWordsComponent implements OnInit {
  @Input() projectId: number;

  constructor(
    private reportService: ReportService,
    private languagesService: LanguageService,
    private userService: UserService,
    private fileService: FileService,
    private projectsService: ProjectsService
  ) {
    //this.selected.from = moment();
    //this.selected.to = moment();
  }

  ngOnInit() {
    console.error("--report ngOnInit-");
    this.languagesService.getLanguageList().subscribe(
      Languages => {
        this.Languages = Languages;
      },
      error => console.error(error)
    );

    this.userService.getUserList().subscribe(
      Users => {
        this.Users = Users;
      },
      error => console.error(error)
    );

    this.fileService.getInitialProjectFolders(this.projectId).subscribe(
      folders => {
        this.Folders = folders;
      },
      error => console.error(error)
    );

    this.projectsService.getProject(this.projectId).subscribe(
      project => {
        this.project = project;
        console.log(this.project.name_text);
        this.selected.from = moment(this.project.date_Of_Creation);
        this.selected.to = moment();
      },
      error => console.error(error)
    );
  }

  public project: LocalizationProject;

  //Строки отчета
  public reportrows: TranslatedWordsReportRow[];

  //Список языков из БД
  public Languages: Locale[];
  //Список пользователей из БД
  public Users: User[];
  //Список папок проекта
  public Folders: File[];

  //Название отчета
  msg: string = "Отчет";

  //Переменные для фильтра выборки
  userName: string;
  userLang: string;

  //Переменные для выборки
  userId: number = -1;
  localeId: number = -1;
  work_Type: string = "Все";
  volumeCalcType: string = "false"; //переделать название переменной
  calcBasisType: string = "true"; //переделать название переменной
  initialFolderId: number = 0;

  //Переменные для фильтьрации
  selected: { from: Moment; to: Moment };

  //Датасорс для грида
  dataSource: MatTableDataSource<TranslatedWordsReportRow>;

  displayedColumns: string[] = [
    "name_text",
    "language",
    "workType",
    "translations"
  ];

  async getRows() {
    console.error("--report getRows-");
    this.reportService
      .getTranslatedWordsReport(
        this.project.id,
        this.selected.from.format("DD.MM.YYYY"),
        this.selected.to.format("DD.MM.YYYY"),
        this.volumeCalcType,
        this.calcBasisType,
        this.userId,
        this.localeId,
        this.work_Type,
        this.initialFolderId
      )
      .subscribe(
        reportrows => (this.reportrows = reportrows),
        error => console.log(error)
      );
  }

  setHeaderMsg() {
    console.error("--report setHeaderMsg-");
    let fromStr = "";
    if (this.selected.from != undefined && this.selected.from != null)
      fromStr = this.selected.from.format("DD-MM-YYYY");
    let toStr = "";
    if (this.selected.to != undefined && this.selected.to != null)
      toStr = " - " + this.selected.to.format("DD-MM-YYYY");
    this.msg = "Лучшие участники:  " + fromStr + toStr;
  }

  getReportPeriod() {
    console.error("--report getReportPeriod-");
    if (this.selected && this.selected.from && this.selected.to)
      return (
        this.selected.from.format("DD.MM.YYYY") +
        " - " +
        this.selected.to.format("DD.MM.YYYY")
      );
  }

  clickEvent() {
    this.getRows();
    this.setHeaderMsg();
  }

  download() {
    console.error("--report getTranslatedWordsReportExcel-");

    this.reportService.getTranslatedWordsReportExcel(
      this.project.id,
      this.selected.from.format("DD.MM.YYYY"),
      this.selected.to.format("DD.MM.YYYY"),
      this.volumeCalcType,
      this.calcBasisType,
      this.userId,
      this.localeId,
      this.work_Type,
      this.initialFolderId
    );
  }
}
