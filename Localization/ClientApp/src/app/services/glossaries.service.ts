import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { Glossary } from 'src/app/models/database-entities/glossary.type';
import { String } from 'src/app/models/database-entities/string.type';
import { Locale } from 'src/app/models/database-entities/locale.type';
import { Term } from 'src/app/models/Glossaries/term.type';

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
    offset?: number,
    sortBy?: string[],
    sortAscending?: boolean): Observable<HttpResponse<Term[]>>
  {
    let params = null;
    let paramsObject: any = {};
    if (termPart && termPart != '') {
      paramsObject.termSearch = termPart;
    }
    if (pageSize) {
      paramsObject.pageSize = pageSize;
    }
    if (offset) {
      paramsObject.offset = offset;
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
      .get<Term[]>(
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

  getGlossaryLocale(glossaryId: number): Observable<Locale> {
    return this.httpClient.get<Locale>(GlossariesService.connectionUrl + glossaryId + '/locale');
  }

  getTranslationLocalesForTerm(glossaryId: number, termId: number): Observable<Locale[]> {
    return this.httpClient.get<Locale[]>(GlossariesService.connectionUrl + glossaryId + '/terms/' + termId + '/locales');
  }

  setTranslationLocalesForTerm(glossaryId: number, termId: number, localesIds: number[]): Observable<Object> {
    return this.httpClient.put(GlossariesService.connectionUrl + glossaryId + '/terms/' + termId + '/locales', localesIds);
  }

}
