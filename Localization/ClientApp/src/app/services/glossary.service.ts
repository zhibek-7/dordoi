import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { Glossaries, GlossariesDTO } from 'src/app/models/DTO/glossaries.type';
//import { Locale } from 'src/app/models/database-entities/locale.type';

@Injectable()
export class GlossaryService
{

  static connectionUrl: string = 'api/glossaries/';

  constructor(private httpClient: HttpClient) { }

  public get(id: number): Observable<Glossaries> {
    return this.httpClient.get<Glossaries>(GlossaryService.connectionUrl + id);
  }

  getGlossaries(): Observable<Glossaries[]> {
    return this.httpClient.get<Glossaries[]>(GlossaryService.connectionUrl);
  } 

  getGlossariesDTO(): Observable<GlossariesDTO[]> {
    return this.httpClient.get<GlossariesDTO[]>(GlossaryService.connectionUrl + "ToDTO");
  }

}
