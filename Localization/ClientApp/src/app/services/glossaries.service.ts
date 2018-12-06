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

  getAssotiatedTermsTotalCount(
    glossaryId: number,
    termPart?: string): Observable<number>
  {
    let params = null;
    let paramsObject: any = {};
    if (termPart && termPart != '') {
      paramsObject.termSearch = termPart;
    }
    if (Object.getOwnPropertyNames(paramsObject).length > 0) {
      params = new HttpParams({
        fromObject: paramsObject
      });
    }
    return this.httpClient.get<number>(
        GlossariesService.connectionUrl + glossaryId + '/terms/count',
        {
          params: params
        });
  }

  getAssotiatedTerms(
    glossaryId: number,
    termPart?: string,
    pageSize?: number,
    pageNumber?: number,
    sortBy?: string[],
    sortAscending?: boolean): Observable<String[]>
  {
    let params = null;
    let paramsObject: any = {};
    if (termPart && termPart != '') {
      paramsObject.termSearch = termPart;
    }
    if (pageSize) {
      paramsObject.pageSize = pageSize;
    }
    if (pageNumber) {
      paramsObject.pageNumber = pageNumber;
    }
    if (sortBy && sortBy.length > 0) {
      let clearedSortBy = new Array<string>();
      for (let sortByElement of sortBy) {
        if (sortByElement)
          clearedSortBy.push(sortByElement);
      }
      if (clearedSortBy.length > 0) {
        paramsObject.sortBy = clearedSortBy;
        if (sortAscending !== undefined) {
          paramsObject.sortAscending = sortAscending;
        }
      }
    }
    if (Object.getOwnPropertyNames(paramsObject).length > 0) {
      params = new HttpParams({
        fromObject: paramsObject
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
