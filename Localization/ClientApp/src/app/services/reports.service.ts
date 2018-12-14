import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { TranslatedWordsReportRow } from "../models/Reports/TranslatedWordsReportRow";
import { Observable} from 'rxjs';

@Injectable()
export class ReportService {

  private url: String = "api/Report/";

  constructor(private httpClient: HttpClient) { }

  //getTranslatedWordsReport(from: string, to: string): Observable<TranslatedWordsReportRow[]> {
  //  return this.httpClient.get<TranslatedWordsReportRow[]>(this.url + 'TranslatedWords:' + from + ":" + to);
  //}

  getTranslatedWordsReport( start: string,
                            end: string,
                            volumeCalcType: string,
                            calcBasisType: string,
                            userId?: number,
                            localeId?: number,
                            workType?: string,
                            initialFolderId?: number ): Observable<TranslatedWordsReportRow[]> {
    
    let paramsObject = "start=" + start;
    paramsObject += "&end=" + end;
    if (volumeCalcType && volumeCalcType != '')
    {
      paramsObject += "&volumeCalcType=" + volumeCalcType;
    }
    if (calcBasisType && calcBasisType != '')
    {
      paramsObject += "&calcBasisType=" + calcBasisType;
    }
    if (workType && workType != '')
    {
      paramsObject += "&workType=" + workType;
    }

    paramsObject += "&userId=" + userId;
    paramsObject += "&localeId=" + localeId;
    paramsObject += "&initialFolderId=" + initialFolderId;

    return this.httpClient.get<TranslatedWordsReportRow[]>(this.url + 'TranslatedWords?' + paramsObject);

  }

  async getTranslatedWordsReportExcel(from: string, to: string) {
        var a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = this.url + 'TranslatedWordsExcel:' + from + ":" + to;
        a.click();
        a.remove(); // remove the element
  }
}
