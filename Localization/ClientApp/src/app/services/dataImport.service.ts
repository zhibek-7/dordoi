import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Locale } from '../models/database-entities/locale.type';

@Injectable({
  providedIn: 'root'
})

export class DataImportService {
  private _url = 'api/dataimport';

  constructor(private http: HttpClient) { }

  importData(file: File, entityName: string, signalrConnectionId: string, cleanTableBeforeImportFlag?: boolean): Observable<Object> {
    const url = `${this._url}/${entityName}`;

    const formData = new FormData();

    formData.append('file', file, file.name);
    if (cleanTableBeforeImportFlag) {
      formData.append('cleanTableBeforeImportFlag', cleanTableBeforeImportFlag.toString());
    }
    else {
      formData.append('cleanTableBeforeImportFlag', 'false');
    }

    if (signalrConnectionId) {
      formData.append('signalrConnectionId', signalrConnectionId);
    }
    else {
      formData.append('signalrConnectionId', '');
    }

    return this.http.post<Object>(url, formData);
  }

}
