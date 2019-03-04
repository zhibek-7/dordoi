import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams  } from "@angular/common/http";
import { Locale } from "../models/database-entities/locale.type";
import { Observable, of } from "rxjs";
import { LocalizationProjectsLocalesDTO } from "../models/DTO/localizationProjectsLocalesDTO";

@Injectable()
export class LanguageService {
  private url = "api/Language/";

  constructor(private httpClient: HttpClient) {}

  getLanguageList(): Observable<Locale[]> {
    return this.httpClient.post<Locale[]>(this.url + "List", null, {
      headers: new HttpHeaders().set(
        "Authorization",
        "Bearer " + sessionStorage.getItem("userToken")
      )
    });
  }

  getByProjectId(projectId: number): Observable<Locale[]> {
    return this.httpClient.post<Locale[]>(
      this.url + "byProjectId/" + projectId,
      projectId,
      {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
  }
    );
  }

  getByUserId(userId: number): Observable<Locale[]> {
    return this.httpClient.post<Locale[]>(
      this.url + "byUserId/" + userId,
      userId,
      {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
  }
    );
  }

  /**
   * Возвращает назначенные языки перевода на проект локализации с процентами переводов по ним.
   * @param projectId Идентификатор проекта локализации.
   */
  getLocalesWithPercentByProjectId(projectId: number): Observable<LocalizationProjectsLocalesDTO[]>
  {
    return this.httpClient.post<LocalizationProjectsLocalesDTO[]>(this.url + "localesWithPercentByProjectId", projectId, {
        headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
      });
  }

  /** Возвращает список языков назначенных на проекты, которые назначены на пользователя. */
  getByUserProjects(): Observable<Locale[]> {
    return this.httpClient.post<Locale[]>(this.url + 'localesByUserProjects', null, {
        headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
      });
  }
  
  /**
   * Возвращает список языков назначенных на память переводов.
   * @param idTranslationMemory идентификатор памяти переводов.
   */
  getByTranslationMemory(idTranslationMemory: number): Observable<Locale[]> {
    return this.httpClient.post<Locale[]>(this.url + 'localesByTranslationMemory', idTranslationMemory, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }

  /**
   * Возвращает список языков назначенных на строки.
   * @param projectId идентификатор проекта локализации.
   * @param idsTranslationSubstring идентификаторы строки.
   */
  getByIdsTranslationSubstring(projectId: number, idsTranslationSubstring: number[]): Observable<Locale[]> {
    let params = new HttpParams();
    params = params.set('projectId', projectId.toString());
    idsTranslationSubstring.forEach(id => params = params.append('idsTranslationSubstring', id.toString()));

    return this.httpClient.post<Locale[]>(this.url + 'localesByIdsTranslationSubstring', null, {
        params: params
    });
  }

  /**
   * Возвращает основной язык проекта.
   * @param projectId идентификатор проекта локализации.
   */
  getSourceLocaleLocalizationProject(projectId: number): Observable<Locale> {
    return this.httpClient.post<Locale>(this.url + "getSourceLocaleLocalizationProject", projectId);
  }
}
