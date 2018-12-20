import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranslatedWordsReportRow } from "../models/Reports/TranslatedWordsReportRow";
import { Observable } from 'rxjs';

@Injectable()
export class ReportService {

  private url: string = "api/Report/";

  constructor(private httpClient: HttpClient) { }

  //getTranslatedWordsReport(from: string, to: string): Observable<TranslatedWordsReportRow[]> {
  //  return this.httpClient.get<TranslatedWordsReportRow[]>(this.url + 'TranslatedWords:' + from + ":" + to);
  //}

  getTranslatedWordsReport(projectId: number,
    start: string,
    end: string,
    volumeCalcType: string,
    calcBasisType: string,
    userId: number,
    localeId: number,
    workType: string,
    initialFolderId: number): Observable<TranslatedWordsReportRow[]> {

    let paramsObject = "projectId=" + projectId;
    paramsObject += "&start=" + start;
    paramsObject += "&end=" + end;
    if (volumeCalcType && volumeCalcType != '') {
      paramsObject += "&volumeCalcType=" + volumeCalcType;
    }
    if (calcBasisType && calcBasisType != '') {
      paramsObject += "&calcBasisType=" + calcBasisType;
    }
    if (workType && workType != '') {
      paramsObject += "&workType=" + workType;
    }

    paramsObject += "&userId=" + userId;
    paramsObject += "&localeId=" + localeId;
    paramsObject += "&initialFolderId=" + initialFolderId;

    return this.httpClient.get<TranslatedWordsReportRow[]>(this.url + 'TranslatedWords?' + paramsObject);

  }

  async getTranslatedWordsReportExcel(
    projectId: number,
    start: string,
    end: string,
    volumeCalcType: string,
    calcBasisType: string,
    userId: number,
    localeId: number,
    workType: string,
    initialFolderId: number) {
    //TODO убрать дубляж кода в заполнении параметров
    let paramsObject = "projectId=" + projectId;
    paramsObject += "&start=" + start;
    paramsObject += "&end=" + end;
    if (volumeCalcType && volumeCalcType != '') {
      paramsObject += "&volumeCalcType=" + volumeCalcType;
    }
    if (calcBasisType && calcBasisType != '') {
      paramsObject += "&calcBasisType=" + calcBasisType;
    }
    if (workType && workType != '') {
      paramsObject += "&workType=" + workType;
    }

    paramsObject += "&userId=" + userId;
    paramsObject += "&localeId=" + localeId;
    paramsObject += "&initialFolderId=" + initialFolderId;

    console.log(paramsObject);

    var a = document.createElement('a');
    document.body.appendChild(a);
    a.setAttribute('style', 'display: none');
    a.href = this.url + 'TranslatedWordsExcel?' + paramsObject;
    a.click();
    a.remove(); // remove the element
  }
}
