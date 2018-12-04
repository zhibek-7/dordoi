import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { Glossary } from 'src/app/models/database-entities/glossary.type';
import { String } from 'src/app/models/database-entities/string.type';

@Injectable()
export class GlossariesService {

  static connectionUrl: string = 'api/glossary/';

  constructor(private httpClient: HttpClient) { }

  public get(id: number): Observable<Glossary> {
    return this.httpClient.get<Glossary>(GlossariesService.connectionUrl + id);
  }

  getGlossaries(): Observable<Glossary[]> {
    return this.httpClient.get<Glossary[]>(GlossariesService.connectionUrl);
  }

  getAssotiatedTerms(glossaryId: number, termPart?: string): Observable<String[]> {
    let params = null;
    if (termPart && termPart != '') {
      params = new HttpParams({
        fromObject:
        {
          'termSearch': termPart
        }
      });
    }
    return this.httpClient
      .get<String[]>(
        GlossariesService.connectionUrl + glossaryId + '/terms',
        {
          params: params
        });
  }

  addNewTerm(glossaryId: number, newTerm: String): Observable<Object> {
    return this.httpClient.post(GlossariesService.connectionUrl + glossaryId + '/terms', newTerm);
  }

  deleteTerm(glossaryId: number, termId: number): Observable<Object> {
    return this.httpClient.delete(GlossariesService.connectionUrl + glossaryId + '/terms/' + termId);
  }

  updateTerm(glossaryId: number, updatedTerm: String): Observable<Object> {
    return this.httpClient.put(GlossariesService.connectionUrl + glossaryId + '/terms/' + updatedTerm.id, updatedTerm);
  }

}
