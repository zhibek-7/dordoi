import { Component, OnInit, ViewChild} from '@angular/core';
import { TranslatedWordsReportRow } from "../../models/Reports/TranslatedWordsReportRow";
import { ReportService } from '../../services/reports.service';
import { LanguageService } from '../../services/languages.service';
import { Language } from '../../models/Language';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';

@Component({
  selector: 'translated-words-report',
  templateUrl: './TranslatedWords.component.html',
  styleUrls: ['./TranslatedWords.component.css']
})

export class TranslatedWordsComponent implements OnInit{
  public reportrows: TranslatedWordsReportRow[];
  public filteredrows: TranslatedWordsReportRow[];
  public Languages: Language[];
  msg: string = "Лучшие участники";
  userName: string;
  userLang: string;
  from: Date;
  to: Date;
  languageList = [];
  dataSource: MatTableDataSource;

  constructor(private reportService: ReportService, private languagesService: LanguageService) {
    this.languagesService.getLanguageList()
      .subscribe( Languages => { this.Languages = Languages; },
                  error => console.error(error));
  }

  displayedColumns: string[] = ['name', 'language', 'translations', 'confirmed'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  ngOnInit() {
    this.dataSource = new MatTableDataSource(this.filteredrows);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }


  async getRows() {
    this.reportService.getTranslatedWordsReport(this.from.toString(), this.to.toString())
      .subscribe( reportrows => { this.filteredrows = reportrows; this.reportrows = reportrows;},
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

