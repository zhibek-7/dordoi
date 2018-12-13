import { Component, ViewChild} from '@angular/core';
import { TranslatedWordsReportRow } from "../../models/Reports/TranslatedWordsReportRow";
import { ReportService } from '../../services/reports.service';
import { LanguageService } from '../../services/languages.service';
import { UserService } from '../../services/user.service';
import { Locale } from '../../models/database-entities/locale.type';
import { MatTableDataSource } from '@angular/material';
import { User } from "../../models/database-entities/user.type";
import { NgxDaterangepickerMd } from 'ngx-daterangepicker-material';
import { Moment } from 'moment';

@Component({
  selector: 'translated-words-report',
  templateUrl: './TranslatedWords.component.html',
  styleUrls: ['./TranslatedWords.component.css']
})

export class TranslatedWordsComponent {
  //Строки отчета
  public reportrows: TranslatedWordsReportRow[];

  //Список языков из БД
  public Languages: Locale[];
  //Список пользователей из БД
  public Users: User[];

  //Название отчета
  msg: string = "Лучшие участники";

  //Переменные для фильтра выборки
  userName: string;
  userLang: string;

  //Переменные для выборки
  userId: number = 0;
  localeId: number = 0;
  workType: string = "Все";
  volumeCalcType: string = "словам";
  calcBasisType: string = "Исходный";
  initialFolderId: number = 0;

  //Переменные для фильтьрации
  selected: { from: Moment, to: Moment };

  //Датасорс для грида
  dataSource: MatTableDataSource<TranslatedWordsReportRow>;

  constructor(private reportService: ReportService, private languagesService: LanguageService, private userService: UserService) {
   
    this.languagesService.getLanguageList()
      .subscribe(Languages => { this.Languages = Languages; },
      error => console.error(error));

    this.userService.getUserList()
      .subscribe(Users => { this.Users = Users; },
        error => console.error(error));
    
  }

  displayedColumns: string[] = ['name', 'language', 'translations', 'confirmed'];

  async getRows() {
    this.reportService.getTranslatedWordsReport(
        this.selected.from.format("DD.MM.YYYY"),
        this.selected.to.format("DD.MM.YYYY"),
        this.volumeCalcType,
        this.calcBasisType,
        this.userId,
        this.localeId,
        this.workType,
        this.initialFolderId)
      .subscribe(
        reportrows => this.reportrows = reportrows,
      error => console.log(error));
  }

  setHeaderMsg() {
    let fromStr = "";
    if (this.selected.from != undefined && this.selected.from != null)
      fromStr = this.selected.from.format("DD-MM-YYYY");
    let toStr = "";
    if (this.selected.to != undefined && this.selected.to != null)
      toStr = " - " + this.selected.to.format("DD-MM-YYYY");
    this.msg = "Лучшие участники:  " + fromStr + toStr;
  }

  clickEvent() {
    this.getRows();
    this.setHeaderMsg();
  }

  download() {
    this.reportService.getTranslatedWordsReportExcel(this.selected.from.format("DD-MM-YYYY"), this.selected.to.format("DD-MM-YYYY"));
  }
}

