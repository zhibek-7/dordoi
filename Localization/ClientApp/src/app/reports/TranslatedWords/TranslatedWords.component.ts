import { Component } from '@angular/core';
import { TranslatedWordsReportRow } from "../../models/Reports/TranslatedWordsReportRow";
import { ReportService } from '../../services/reports.service';
import { LanguageService } from '../../services/languages.service';
import { Language  } from '../../models/Language';

@Component({
  selector: 'translated-words-report',
  templateUrl: './TranslatedWords.component.html',
  styleUrls: ['./TranslatedWords.component.css']
})

export class TranslatedWordsComponent{
  public reportrows: TranslatedWordsReportRow[];
  public filteredrows: TranslatedWordsReportRow[];
  public Languages: Language[];
  msg: string = "Лучшие участники";
  userName: string;
  userLang: string;
  from: Date;
  to: Date;
  languageList = [];

  constructor(private reportService: ReportService, private languagesService: LanguageService) {
    this.languagesService.getLanguageList()
      .subscribe( Languages => { this.Languages = Languages; },
                  error => console.error(error));
  }
  
  async getRows() {
    this.reportService.getTranslatedWordsReport(this.from.toString(), this.to.toString())
      .subscribe( reportrows => { this.filteredrows = reportrows; this.reportrows = reportrows;},
      error => console.error(error));
  }

  filterReport() {
    let rows: TranslatedWordsReportRow[] = new Array();
    for (var i = 0; i < this.reportrows.length; i++) {
      if ((this.userName == undefined || this.userName == null || this.reportrows[i].name.indexOf(this.userName) != -1)
        && (this.userLang === "Все языки" || this.userLang == undefined || this.reportrows[i].language.indexOf(this.userLang) != -1))
        rows.push(new TranslatedWordsReportRow(this.reportrows[i].name, this.reportrows[i].language, this.reportrows[i].translations, this.reportrows[i].confirmed));
    }
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

