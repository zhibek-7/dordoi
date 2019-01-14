import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { Glossaries, GlossariesDTO } from 'src/app/models/DTO/glossaries.type';

import { Locale } from 'src/app/models/database-entities/locale.type';
import { localizationProjectForSelectDTO } from 'src/app/models/DTO/localizationProjectForSelectDTO.type';

@Injectable()
export class GlossaryService
{

  static connectionUrl: string = 'api/glossaries/';

  constructor(private httpClient: HttpClient) { }

  //public get(id: number): Observable<Glossaries>
  //{
  //  return this.httpClient.post<Glossaries>(GlossaryService.connectionUrl + id);
  //}

  getGlossariesDTO(): Observable<GlossariesDTO[]>
  {
    return this.httpClient.get<GlossariesDTO[]>(GlossaryService.connectionUrl);
  }

  getLocales(): Observable<Locale[]> {
    return this.httpClient.get<Locale[]>(GlossaryService.connectionUrl + "locales/list");
  }

  getlocalizationProjectForSelectDTO(): Observable<localizationProjectForSelectDTO[]> {
    return this.httpClient.get<localizationProjectForSelectDTO[]>(GlossaryService.connectionUrl + "localizationProjects/list");
  }
}
