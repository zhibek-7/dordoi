import { Injectable } from '@angular/core';
//import { Http, ResponseContentType } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { TranslatedWordsReportRow } from "../models/Reports/TranslatedWordsReportRow";

@Injectable()
export class ReportService {

  private url: String = document.getElementsByTagName('base')[0].href + "api/Report/";

  constructor(//private http: Http,
    private httpClient: HttpClient) { }

  async getTranslatedWordsReport(from: string, to: string) {
    let reportRows: TranslatedWordsReportRow[] = await this.httpClient.get<TranslatedWordsReportRow[]>(this.url + 'TranslatedWords:' + from + ":" + to).toPromise();
    return reportRows;
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
