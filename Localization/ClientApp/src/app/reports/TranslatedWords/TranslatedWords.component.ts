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
  //Фильтрованные строки отчета
  public filteredrows: TranslatedWordsReportRow[];

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
  user: string = "Все";
  locale: string = "Все";
  workType: string = "Все";
  volumeCalcType: string = "словам";
  calcBasisType: string = "Исходный";
  initialFolder: string;

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
    this.reportService.getTranslatedWordsReport(this.selected.from.format("DD-MM-YYYY"), this.selected.to.format("DD-MM-YYYY"))
      .subscribe(reportrows => {
          this.filteredrows = reportrows;
        this.reportrows = reportrows;
        },
      error => console.error(error));   
  }

  filterReport() {
    let rows: TranslatedWordsReportRow[] = this.reportrows;

    if (this.userName != undefined && this.userName != null)
      rows = rows.filter(s => s.name.indexOf(this.userName) != -1);

    if (this.userLang != "Все" && this.userLang != undefined)
      rows = rows.filter(s => s.language.indexOf(this.userLang) != -1);

    this.filteredrows = rows;
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
    while (!this.getRows())
      this.filterReport();
    this.setHeaderMsg();
  }

  confimed(val: string) : string {
    if (val === "true") return "Да";
    else return "Нет";
  }

  download() {
    this.reportService.getTranslatedWordsReportExcel(this.selected.from.format("DD-MM-YYYY"), this.selected.to.format("DD-MM-YYYY"));
  }
}

