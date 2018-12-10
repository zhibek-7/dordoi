import { Component, ViewChild} from '@angular/core';
import { TranslatedWordsReportRow } from "../../models/Reports/TranslatedWordsReportRow";
import { ReportService } from '../../services/reports.service';
import { LanguageService } from '../../services/languages.service';
import { UserService } from '../../services/user.service';
import { Locale } from '../../models/database-entities/locale.type';
import { MatTableDataSource } from '@angular/material';
import { User} from "../../models/database-entities/user.type";

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

  //Переменные для выборки
  userName: string;
  userLang: string;

  //Переменные для фильтьрации
  from: Date;
  to: Date;

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
    this.reportService.getTranslatedWordsReport(this.from.toString(), this.to.toString())
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

    if (this.userLang != "Все языки" && this.userLang != undefined)
      rows = rows.filter(s => s.language.indexOf(this.userLang) != -1);

    this.filteredrows = rows;
  }

  setHeaderMsg() {
    let fromStr = "";
    if (this.from != undefined && this.from != null)
      fromStr = this.from.toString();
    let toStr = "";
    if (this.to != undefined && this.to != null)
      toStr = " - " + this.to;
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
    this.reportService.getTranslatedWordsReportExcel(this.from.toString(), this.to.toString());
  }
}

