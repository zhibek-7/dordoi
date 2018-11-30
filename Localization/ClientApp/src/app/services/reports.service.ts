import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranslatedWordsReportRow } from "../models/Reports/TranslatedWordsReportRow";

@Injectable()
export class ReportService {

  private url: String = document.getElementsByTagName('base')[0].href + "api/Report/";

  constructor(private http: HttpClient) {}

  async getTranslatedWordsReport(from: string, to: string) {
    let reportRows: TranslatedWordsReportRow[] = await this.http.get<TranslatedWordsReportRow[]>(this.url + 'TranslatedWords:' + from + ":" + to).toPromise();
    return reportRows;
  }
}
