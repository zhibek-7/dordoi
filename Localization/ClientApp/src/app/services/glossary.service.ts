import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { GlossariesForEditing, GlossariesTableViewDTO } from 'src/app/models/DTO/glossariesDTO.type';


@Injectable()
export class GlossaryService {
  static connectionUrl: string = 'api/glossaries/';

  constructor(private httpClient: HttpClient) { }


  /**
   * Возвращает список глоссариев, со строками перечислений имен связанных объектов.
   */
  getGlossariesDTO(): Observable<GlossariesTableViewDTO[]> {
    return this.httpClient.post<GlossariesTableViewDTO[]>(GlossaryService.connectionUrl, null);
  }  
  
  /**
   * Добавление нового глоссария.
   * @param glossary Новый глоссарий.
   */
  addNewGlossary(glossary: GlossariesForEditing): Observable<Object> {
    return this.httpClient.post(GlossaryService.connectionUrl + "newGlossary", glossary);
  }
  
  /**
   * Возвращает глоссарий для редактирования (со связанными объектами).
   * @param glossaryId Идентификатор глоссария.
   */
  async getGlossaryForEditing(glossaryId: number): Promise<GlossariesForEditing> {
    let result = await this.httpClient.post<GlossariesForEditing>(GlossaryService.connectionUrl + "edit", glossaryId).toPromise();
    return result;
  }
  
  /**
   * Сохранение изменений в глоссарии.
   * @param glossary Отредактированный глоссарий.
   */
  editSaveGlossary(glossary: GlossariesForEditing): Observable<Object> {
    return this.httpClient.post(GlossaryService.connectionUrl + "editSaveGlossary", glossary);
  }
  
  /**
   * Удаление глоссария.
   * @param glossaryId Идентификатор глоссария.
   */
  deleteGlossary(glossaryId: number): Observable<Object> {
    return this.httpClient.delete(GlossaryService.connectionUrl + "deleteGlossary/" + glossaryId);
  }

  // 
  /**
   * Удаление всех терминов глоссария.
   * @param glossaryId Идентификатор глоссария.
   */
  clearGlossary(glossaryId: number): Observable<Object> {
    return this.httpClient.delete(GlossaryService.connectionUrl + "clearGlossary/" + glossaryId);
  }
}
