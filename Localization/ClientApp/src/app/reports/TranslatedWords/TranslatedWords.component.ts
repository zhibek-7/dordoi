import { Component} from '@angular/core';

import { TranslatedWordsReportRow } from "../../models/Reports/TranslatedWordsReportRow";
import { ReportService } from '../../services/reports.service';

@Component({
  selector: 'translated-words-report',
  templateUrl: './TranslatedWords.component.html',
  styleUrls: ['./TranslatedWords.component.css']
})

export class TranslatedWordsComponent{
  public reportrows: TranslatedWordsReportRow[];
  msg: string = "Лучшие участники";
  from: Date;
  to: Date;

  constructor(private reportService: ReportService) {
  }

  async getRows() {
    this.reportrows = await this.reportService.getTranslatedWordsReport(this.from.toString(), this.to.toString());
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
    this.getRows();
    this.setHeaderMsg();
  }

  ConvertToCSV(objArray) {
    var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
    var str = '';
    var row = "";

    for (var index in objArray[0]) {
      //Now convert each value to string and comma-separated
      row += index + ',';
    }
    row = row.slice(0, -1);
    //append Label row with line break
    str += row + '\r\n';

    for (var i = 0; i < array.length; i++) {
      var line = '';
      for (var index in array[i]) {
        if (line != '') line += ','

        line += array[i][index];
      }
      str += line + '\r\n';
    }
    return str;
  }

  download() {
    var csvData = this.ConvertToCSV(this.reportrows);
    var a = document.createElement("a");
    a.setAttribute('style', 'display:none;');
    document.body.appendChild(a);
    var blob = new Blob([csvData], { type: 'application/vnd.ms-excel' });
    var url = window.URL.createObjectURL(blob);
    a.href = url;
    a.download = 'User_Results.xls';/* your file name*/
    a.click();
    return 'success';
  }
}

