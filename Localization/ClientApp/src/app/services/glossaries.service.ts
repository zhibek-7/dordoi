import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { Glossary } from '../models/database-entities/glossary.type';
import { String } from '../models/database-entities/string.type';

@Injectable()
export class GlossariesService {

  static connectionUrl: string = 'api/glossary/';

  constructor(private httpClient: HttpClient) { }

  glossaries: Glossary[];

  public get(id: number): Observable<Glossary> {
    return this.httpClient.get<Glossary>(GlossariesService.connectionUrl + id.toString());
  }

  getGlossaries(): Observable<Glossary[]> {
    return this.httpClient.get<Glossary[]>(GlossariesService.connectionUrl);
  }

  getAssotiatedTerms(glossaryId: number): Observable<String[]> {
    return this.httpClient.get<String[]>(GlossariesService.connectionUrl + glossaryId.toString() + '/get_assotiated_terms');
  }

}
