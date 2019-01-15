import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Locale } from '../models/database-entities/locale.type';
import { Observable, of } from 'rxjs';

@Injectable()
export class LanguageService {

  private url = 'api/Language/';

    constructor(private httpClient: HttpClient) { }

  getLanguageList(): Observable<Locale[]> {
    return this.httpClient.get<Locale[]>(this.url + 'List');
  }

  getByProjectId(projectId: number): Observable<Locale[]> {
    return this.httpClient.get<Locale[]>(this.url + 'byProjectId/' + projectId);
  }

  getByUserId(userId: number): Observable<Locale[]> {
    return this.httpClient.get<Locale[]>(this.url + 'byUserId/' + userId);
  }

}
