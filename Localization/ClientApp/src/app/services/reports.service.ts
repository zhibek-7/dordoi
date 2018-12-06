import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranslatedWordsReportRow } from "../models/Reports/TranslatedWordsReportRow";
import { Observable} from 'rxjs';

@Injectable()
export class ReportService {

  private url: String = "api/Report/";

  constructor(private httpClient: HttpClient) { }

  getTranslatedWordsReport(from: string, to: string): Observable<TranslatedWordsReportRow[]> {
    return this.httpClient.get<TranslatedWordsReportRow[]>(this.url + 'TranslatedWords:' + from + ":" + to);
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
