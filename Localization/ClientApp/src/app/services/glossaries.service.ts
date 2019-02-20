import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse, HttpHeaders  } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { Glossary } from 'src/app/models/database-entities/glossary.type';
import { TranslationSubstring } from 'src/app/models/database-entities/translationSubstring.type';
import { Locale } from 'src/app/models/database-entities/locale.type';
import { Term } from 'src/app/models/Glossaries/term.type';
import { TermWithGlossary } from '../work-panel/localEntites/terms/termWithGlossary.type';

@Injectable()
export class GlossariesService {

  static connectionUrl: string = 'api/glossary/';

  constructor(private httpClient: HttpClient) { }

  private handleError<T>(error: any, response: Observable<T>): Observable<T> {
    console.log(error);
    return of(undefined as T);
  }

  public get(id: number): Observable<Glossary> {
    return this.httpClient.post<Glossary>(GlossariesService.connectionUrl + id + '/get', null, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    })
      .pipe(catchError(this.handleError));
  }

  getGlossaries(): Observable<Glossary[]> {
    return this.httpClient.post<Glossary[]>(GlossariesService.connectionUrl + 'list', null, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    })
      .pipe(catchError(this.handleError));
  }

  getAssotiatedTerms(
    glossaryId: number,
    termPart?: string,
    limit?: number,
    offset?: number,
    sortBy?: string[],
    sortAscending?: boolean): Observable<HttpResponse<Term[]>>
  {
    let body: any = { };
    if (termPart && termPart != '') {
      body.termSearch = termPart;
    }
    if (limit) {
      body.limit = limit;
    }
    if (offset) {
      body.offset = offset;
    }
    if (sortBy && sortBy.length > 0) {
      body.sortBy = sortBy;
      if (sortAscending !== undefined) {
        body.sortAscending = sortAscending;
      }
    }
    return this.httpClient
      .post<Term[]>(
        GlossariesService.connectionUrl + glossaryId + '/terms/list',
        body,
        {
          observe: 'response',
        })
      .pipe(catchError(this.handleError));
  }

  addNewTerm(glossaryId: number, newTerm: TranslationSubstring, partOfSpeechId: number | null): Observable<Object> {
    let params = new HttpParams();
    if (partOfSpeechId !== null) {
      params = params.set('partOfSpeechId', partOfSpeechId.toString());
    }
    return this.httpClient.post(GlossariesService.connectionUrl + glossaryId + '/terms', newTerm,
      {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken")), params: params
    })
      .pipe(catchError(this.handleError));
  }

  deleteTerm(glossaryId: number, termId: number): Observable<Object> {
    return this.httpClient.delete(GlossariesService.connectionUrl + glossaryId + '/terms/' + termId, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    })
      .pipe(catchError(this.handleError));
  }

  updateTerm(glossaryId: number, updatedTerm: TranslationSubstring, partOfSpeechId: number | null): Observable<Object> {
    let params = new HttpParams();
    if (partOfSpeechId !== null) {
      params = params.set('partOfSpeechId', partOfSpeechId.toString());
    }
    return this.httpClient.put(GlossariesService.connectionUrl + glossaryId + '/terms/' + updatedTerm.id, updatedTerm,
      {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken")), params: params
    })
      .pipe(catchError(this.handleError));
  }

  getGlossaryLocale(glossaryId: number): Observable<Locale> {
    return this.httpClient.post<Locale>(GlossariesService.connectionUrl + glossaryId + '/locale/get', null, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    })
      .pipe(catchError(this.handleError));
  }

  getTranslationLocalesForTerm(glossaryId: number, termId: number): Observable<Locale[]> {
    return this.httpClient.post<Locale[]>(GlossariesService.connectionUrl + glossaryId + '/terms/' + termId + '/locales/list', null, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    })
      .pipe(catchError(this.handleError));
  }

  setTranslationLocalesForTerm(glossaryId: number, termId: number, localesIds: number[]): Observable<Object> {
    return this.httpClient.put(GlossariesService.connectionUrl + glossaryId + '/terms/' + termId + '/locales', localesIds, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    })
      .pipe(catchError(this.handleError));
  }

  // Получить все термины из всех глоссариев присоедененных к проекту локализации, по id необходимого проекта локализации
  getAllTermsFromAllGlossarisInProject(projectId: number): Observable<TermWithGlossary[]> {
    return this.httpClient.post<TermWithGlossary[]>(GlossariesService.connectionUrl + 'FindAllTermsInProjects/', projectId, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    })
      .pipe(catchError(this.handleError));
  }

}
