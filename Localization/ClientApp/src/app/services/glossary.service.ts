import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { GlossariesForEditing, GlossariesTableViewDTO } from 'src/app/models/DTO/glossariesDTO.type';

import { Locale } from 'src/app/models/database-entities/locale.type';
import { LocalizationProjectForSelectDTO } from 'src/app/models/DTO/localizationProjectForSelectDTO.type';

@Injectable()
export class GlossaryService {
  static connectionUrl: string = 'api/glossaries/';

  constructor(private httpClient: HttpClient) { }

  //Возвращает список глоссариев, со строками перечислений имен связанных объектов
  getGlossariesDTO(): Observable<GlossariesTableViewDTO[]> {
    return this.httpClient.post<GlossariesTableViewDTO[]>(GlossaryService.connectionUrl, null);
  }

  ////Возвращает список языков переводов
  //getLocales(): Observable<Locale[]> {
  //  return this.httpClient.post<Locale[]>(GlossaryService.connectionUrl + "locales/list", null);
  //}

  ////Возвращает список проектов локализации
  //getLocalizationProjectForSelectDTO(): Observable<LocalizationProjectForSelectDTO[]> {
  //  return this.httpClient.post<LocalizationProjectForSelectDTO[]>(GlossaryService.connectionUrl + "localizationProjects/list", null);
  //}

  //Добавление нового глоссария
  addNewGlossary(glossary: GlossariesForEditing): Observable<Object> {
    return this.httpClient.post(GlossaryService.connectionUrl + "newGlossary", glossary);
  }

  //Возвращает глоссарий для редактирования (со связанными объектами)
  async getGlossaryForEditing(glossaryId: number): Promise<GlossariesForEditing> { //Проверить возможно где-то косяк, из-за которого требуется явный async-await
    let result = await this.httpClient.post<GlossariesForEditing>(GlossaryService.connectionUrl + "edit", glossaryId).toPromise();
    return result;
  }

  //Сохранение изменений в глоссарии
  editSaveGlossary(glossary: GlossariesForEditing): Observable<Object> {
    return this.httpClient.post(GlossaryService.connectionUrl + "editSaveGlossary", glossary);
  }

  //Удаление глоссария
  deleteGlossary(glossaryId: number): Observable<Object> {
    return this.httpClient.delete(GlossaryService.connectionUrl + "deleteGlossary/" + glossaryId);
  }

  //Удаление всех терминов глоссария
  clearGlossary(glossaryId: number): Observable<Object> {
    return this.httpClient.delete(GlossaryService.connectionUrl + "clearGlossary/" + glossaryId);
  }
}
