import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
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

  getAssotiatedTerms(
    glossaryId: number,
    termPart?: string,
    pageSize?: number,
    pageNumber?: number,
    sortBy?: string[],
    sortAscending?: boolean): Observable<HttpResponse<String[]>>
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
          params: params,
          observe: 'response'
        });
  }

  addNewTerm(glossaryId: number, newTerm: String, partOfSpeechId: number | null): Observable<Object> {
    let params = null;
    let paramsObject: any = {};
    if (partOfSpeechId !== null) {
      paramsObject.partOfSpeechId = partOfSpeechId;
    }
    if (Object.getOwnPropertyNames(paramsObject).length > 0) {
      params = new HttpParams({
        fromObject: paramsObject
      });
    }
    return this.httpClient.post(GlossariesService.connectionUrl + glossaryId + '/terms', newTerm,
      {
        params: params
      });
  }

  deleteTerm(glossaryId: number, termId: number): Observable<Object> {
    return this.httpClient.delete(GlossariesService.connectionUrl + glossaryId + '/terms/' + termId);
  }

  updateTerm(glossaryId: number, updatedTerm: String, partOfSpeechId: number | null): Observable<Object> {
    let params = null;
    let paramsObject: any = {};
    if (partOfSpeechId !== null) {
      paramsObject.partOfSpeechId = partOfSpeechId;
    }
    if (Object.getOwnPropertyNames(paramsObject).length > 0) {
      params = new HttpParams({
        fromObject: paramsObject
      });
    }
    return this.httpClient.put(GlossariesService.connectionUrl + glossaryId + '/terms/' + updatedTerm.id, updatedTerm,
      {
        params: params
      });
  }

  getTermPartOfSpeechId(glossaryId: number, termId: number): Observable<number> {
    return this.httpClient.get<number>(GlossariesService.connectionUrl + glossaryId + '/terms/' + termId + '/part_of_speech');
  }

}
