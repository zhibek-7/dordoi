import { Component, ViewChild} from '@angular/core';
import { TranslatedWordsReportRow } from "../../models/Reports/TranslatedWordsReportRow";
import { ReportService } from '../../services/reports.service';
import { LanguageService } from '../../services/languages.service';
import { Language } from '../../models/Language';
import { MatTableDataSource } from '@angular/material';

@Component({
  selector: 'translated-words-report',
  templateUrl: './TranslatedWords.component.html',
  styleUrls: ['./TranslatedWords.component.css']
})

export class TranslatedWordsComponent {
  public reportrows: TranslatedWordsReportRow[];
  public filteredrows: TranslatedWordsReportRow[];
  public Languages: Language[];
  msg: string = "Лучшие участники";
  userName: string;
  userLang: string;
  from: Date;
  to: Date;
  languageList = [];
  dataSource: MatTableDataSource<TranslatedWordsReportRow>;

  constructor(private reportService: ReportService, private languagesService: LanguageService) {
   
    this.languagesService.getLanguageList()
      .subscribe(Languages => { this.Languages = Languages; },
      error => console.error(error));
    
  }

  displayedColumns: string[] = ['name', 'language', 'translations', 'confirmed'];

  ELEMENT_DATA: TranslatedWordsReportRow[] = [
    { language: "Английский", name: 'Hydrogen', translations:  "1.0079", confirmed: "true" },
    { language: "Английский", name: 'Helium', translations:  "4.0026", confirmed: "true" },
    { language: "Английский", name: 'Lithium', translations:  "6.941", confirmed: "true" },
    { language: "Английский", name: 'Beryllium', translations:  "9.0122", confirmed: "true" },
    { language: "Английский", name: 'Boron', translations:  "10.811", confirmed: "true" },
    { language: "Английский", name: 'Carbon', translations:  "12.0107", confirmed: "true" },
    { language: "Английский", name: 'Nitrogen', translations:  "14.0067", confirmed: "true" },
    { language: "Английский", name: 'Oxygen', translations:  "15.9994", confirmed: "true" },
    { language: "Английский", name: 'Fluorine', translations:  "18.9984", confirmed: "true" },
    { language: "Английский", name: 'Neon', translations:  "20.1797", confirmed: "true" },
    { language: "Английский", name: 'Soldium', translations:  "1.0079", confirmed: "true" },
    { language: "Английский", name: 'Magnesium', translations:  "4.0026", confirmed: "true" },
    { language: "Английский", name: 'Aluminium', translations:  "6.941", confirmed: "true" },
    { language: "Английский", name: 'Silicon', translations:  "9.0122", confirmed: "true" },
    { language: "Английский", name: 'Phosphorous', translations:  "10.811", confirmed: "true" },
  ];

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

