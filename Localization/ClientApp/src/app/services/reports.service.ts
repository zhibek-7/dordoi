//import { Params } from "@angular/router";
import { Injectable } from "@angular/core";
import { HttpClient,  HttpHeaders, HttpParams } from "@angular/common/http";
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

  getTranslatedWordsReportExcel(
    projectId: Guid,
    start: string,
    end: string,
    volumeCalcType: string,
    calcBasisType: string,
    userId: Guid,
    localeId: Guid,
    workType: string,
    initialFolderId: Guid
    ): Observable<Blob> {

    //TODO убрать дубляж кода в заполнении параметров
      
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
      
      return this.httpClient.post(this.url + "TranslatedWordsExcel", body,
      {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        ),
        responseType: "blob"
      });
  }
}
