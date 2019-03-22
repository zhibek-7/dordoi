import { Injectable } from "@angular/core";
import {
  HttpClient,
  HttpParams,
  HttpResponse,
  HttpHeaders
} from "@angular/common/http";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";

import { Glossary } from "src/app/models/database-entities/glossary.type";
import { TranslationSubstring } from "src/app/models/database-entities/translationSubstring.type";
import { Locale } from "src/app/models/database-entities/locale.type";
import { Term } from "src/app/models/Glossaries/term.type";
import { TermWithGlossary } from "../work-panel/localEntites/terms/termWithGlossary.type";
import { Guid } from "guid-typescript";

@Injectable()
export class GlossariesService {
  static connectionUrl: string = "api/glossary/";

  constructor(private httpClient: HttpClient) {}

  private handleError<T>(error: any, response: Observable<T>): Observable<T> {
    console.log(error);
    return of(undefined as T);
  }

  public get(id: Guid): Observable<Glossary> {
    return this.httpClient
      .post<Glossary>(GlossariesService.connectionUrl + id + "/get", null)
      .pipe(catchError(this.handleError));
  }

  getGlossaries(): Observable<Glossary[]> {
    return this.httpClient
      .post<Glossary[]>(GlossariesService.connectionUrl + "list", null)
      .pipe(catchError(this.handleError));
  }

  getAssotiatedTerms(
    glossaryId: Guid,
    termPart?: string,
    limit?: number,
    offset?: number,
    sortBy?: string[],
    sortAscending?: boolean
  ): Observable<HttpResponse<Term[]>> {
    let body: any = {};
    if (termPart && termPart != "") {
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
        GlossariesService.connectionUrl + glossaryId + "/terms/list",
        body,
        {
          headers: new HttpHeaders().set(
            "Authorization",
            "Bearer " + sessionStorage.getItem("userToken")
          ),
          observe: "response"
        }
      )
      .pipe(catchError(this.handleError));
  }

  addNewTerm(
    glossaryId: Guid,
    newTerm: TranslationSubstring,
    partOfSpeechId: Guid | null
  ): Observable<Object> {
    let params = new HttpParams();
    if (partOfSpeechId !== null) {
      params = params.set("partOfSpeechId", partOfSpeechId.toString());
    }
    return this.httpClient
      .post(GlossariesService.connectionUrl + glossaryId + "/terms", newTerm)
      .pipe(catchError(this.handleError));
  }

  deleteTerm(glossaryId: Guid, termId: Guid): Observable<Object> {
    return this.httpClient
      .delete(GlossariesService.connectionUrl + glossaryId + "/terms/" + termId)
      .pipe(catchError(this.handleError));
  }

  updateTerm(
    glossaryId: Guid,
    updatedTerm: TranslationSubstring,
    partOfSpeechId: Guid | null
  ): Observable<Object> {
    let params = new HttpParams();
    if (partOfSpeechId !== null) {
      params = params.set("partOfSpeechId", partOfSpeechId.toString());
    }
    return this.httpClient
      .put(
        GlossariesService.connectionUrl +
          glossaryId +
          "/terms/" +
          updatedTerm.id,
        updatedTerm
      )
      .pipe(catchError(this.handleError));
  }

  getGlossaryLocale(glossaryId: Guid): Observable<Locale> {
    return this.httpClient
      .post<Locale>(
        GlossariesService.connectionUrl + glossaryId + "/locale/get",
        null
      )
      .pipe(catchError(this.handleError));
  }

  getGlossaryLocaleByTerm(termId?: Guid): Observable<Locale> {
    console.log("getGlossaryLocaleByTerm==" + termId);
    if (termId == null || termId == Guid.createEmpty()) {
      return this.httpClient
        .post<Locale>(
          GlossariesService.connectionUrl +
            "term/locale/get/" +
            Guid.createEmpty(),
          null
        )
        .pipe(catchError(this.handleError));
    } else {
      return this.httpClient
        .post<Locale>(
          GlossariesService.connectionUrl + "term/locale/get/" + termId,
          null
        )
        .pipe(catchError(this.handleError));
    }
  }

  getTranslationLocalesForTerm(
    glossaryId: Guid,
    termId: Guid
  ): Observable<Locale[]> {
    return this.httpClient
      .post<Locale[]>(
        GlossariesService.connectionUrl + "terms/" + termId + "/locales/list",
        null
      )
      .pipe(catchError(this.handleError));
  }

  setTranslationLocalesForTerm(
    glossaryId: Guid,
    termId: Guid,
    localesIds: Guid[]
  ): Observable<Object> {
    return this.httpClient
      .put(
        GlossariesService.connectionUrl +
          glossaryId +
          "/terms/" +
          termId +
          "/locales",
        localesIds
      )
      .pipe(catchError(this.handleError));
  }

  // Получить все термины из всех глоссариев присоедененных к проекту локализации, по id необходимого проекта локализации
  getAllTermsFromAllGlossarisInProject(
    projectId: Guid
  ): Observable<TermWithGlossary[]> {
    return this.httpClient
      .post<TermWithGlossary[]>(
        GlossariesService.connectionUrl + "FindAllTermsInProjects/" + projectId,
        projectId
      )
      .pipe(catchError(this.handleError));
  }
}
