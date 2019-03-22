//import { Params } from "@angular/router";
import { Injectable } from "@angular/core";
import { HttpClient,  HttpHeaders } from "@angular/common/http";
import { TranslatedWordsReportRow } from "../models/Reports/TranslatedWordsReportRow";
import { Observable } from "rxjs";
import { Guid } from 'guid-typescript';
@Injectable()
export class ReportService {
  private url: string = "api/Report/";

  constructor(private httpClient: HttpClient) {}

  //getTranslatedWordsReport(from: string, to: string): Observable<TranslatedWordsReportRow[]> {
  //  return this.httpClient.get<TranslatedWordsReportRow[]>(this.url + 'TranslatedWords:' + from + ":" + to);
  //}

  getTranslatedWordsReport(
    projectId: Guid,
    start: string,
    end: string,
    volumeCalcType: string,
    calcBasisType: string,
    userId: Guid,
    localeId: Guid,
    workType: string,
    initialFolderId: Guid
  ): Observable<TranslatedWordsReportRow[]> {
    const body = {
      projectId: projectId,
      start: start,
      end: end,
      volumeCalcType: volumeCalcType,
      calcBasisType: calcBasisType,
      workType: workType,
      userId: userId,
      localeId: localeId,
      initialFolderId: initialFolderId
    };
    return this.httpClient.post<TranslatedWordsReportRow[]>(
      this.url + "TranslatedWords",
      body,
      {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      }
    );
  }

  async getTranslatedWordsReportExcel(
    projectId: Guid,
    start: string,
    end: string,
    volumeCalcType: string,
    calcBasisType: string,
    userId: Guid,
    localeId: Guid,
    workType: string,
    initialFolderId: Guid
  ) {
    //TODO убрать дубляж кода в заполнении параметров
    let paramsObject = "projectId=" + projectId;
    paramsObject += "&start=" + start;
    paramsObject += "&end=" + end;
    if (volumeCalcType && volumeCalcType != "") {
      paramsObject += "&volumeCalcType=" + volumeCalcType;
    }
    if (calcBasisType && calcBasisType != "") {
      paramsObject += "&calcBasisType=" + calcBasisType;
    }
    if (workType && workType != "") {
      paramsObject += "&workType=" + workType;
    }

    paramsObject += "&userId=" + userId;
    paramsObject += "&localeId=" + localeId;
    paramsObject += "&initialFolderId=" + initialFolderId;

    console.log(paramsObject);

    var a = document.createElement("a");
    document.body.appendChild(a);
    //TODO нужно переделать, авторизация не работает
    a.setAttribute("style", "display: none");
    a.href = this.url + "TranslatedWordsExcel?" + paramsObject;

    a.click();
    a.remove(); // remove the element
  }
}
