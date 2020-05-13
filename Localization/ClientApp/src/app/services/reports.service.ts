//import { Params } from "@angular/router";
import { Injectable } from "@angular/core";
import { HttpClient,  HttpHeaders, HttpParams } from "@angular/common/http";
import { TranslatedWordsReportRow } from "../models/Reports/TranslatedWordsReportRow";
import { Observable } from "rxjs";
import { Guid } from 'guid-typescript';


import { Locale } from "../models/database-entities/locale.type";
import { User } from "../models/database-entities/user.type";
import { TypeOfServiceForSelectDTO } from '../models/DTO/typeOfServiceForSelectDTO.type';

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
    user: User,
    locale: Locale,
    workType: TypeOfServiceForSelectDTO,
    initialFolderId: Guid
  ): Observable<TranslatedWordsReportRow[]> {
    const body = {
      projectId: projectId,
      start: start,
      end: end,
      volumeCalcType: volumeCalcType,
      calcBasisType: calcBasisType,
      workType: workType,
      user: user,
      locale: locale,
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
    user: User,
    locale: Locale,
    workType: TypeOfServiceForSelectDTO,
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
      user: user,
      locale: locale,
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
