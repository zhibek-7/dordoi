import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpResponse } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { Guid } from 'guid-typescript';

import {
  GlossariesForEditing,
  GlossariesTableViewDTO
} from "src/app/models/DTO/glossariesDTO.type";
import { catchError } from "rxjs/operators";

@Injectable()
export class GlossaryService {
  static connectionUrl: string = "api/glossaries/";

  constructor(private httpClient: HttpClient) {}


  /**
   * Возвращает список словарей текущего пользователя, со строками перечислений имен связанных объектов.
   * @param projectId Идентификатор проекта.
   * @param searchString Шаблон названия словаря (поиск по name_text).
   * @param limit Количество возвращаемых строк.
   * @param offset Количество пропущенных строк.
   * @param sortBy Имя сортируемого столбца.
   * @param sortAscending Порядок сортировки.
   */
  getAllByUserId(
    projectId?: Guid,
    searchString?: string,
    limit?: number,
    offset?: number,
    sortBy?: string[],
    sortAscending?: boolean): Observable<HttpResponse<GlossariesTableViewDTO[]>> {
    let params = new HttpParams();
    if (searchString && searchString != '') {
      params = params.set('searchString', searchString);
    }
    if (projectId) {
      params = params.set('projectId', projectId.toString());
    }
    if (limit) {
      params = params.set('limit', limit.toString());
    }
    if (offset) {
      params = params.set('offset', offset.toString());
    }
    if (sortBy && sortBy.length > 0) {
      sortBy.forEach(sortByItem => params = params.append('sortBy', sortByItem));
      if (sortAscending !== undefined) {
        params = params.set('sortAscending', sortAscending.toString());
      }
    }
    return this.httpClient.post<GlossariesTableViewDTO[]>(GlossaryService.connectionUrl + "byUserId", null,
      {
        params: params,
        observe: 'response'
      });
  }

  /**
   * Возвращает словарь для редактирования (со связанными объектами).
   * @param glossaryId Идентификатор словаря.
   */
  async getGlossaryForEditing(
    glossaryId: Guid
  ): Promise<GlossariesForEditing> {
	 console.log(" getGlossaryForEditing==" + glossaryId);

    let result = await this.httpClient
      .post<GlossariesForEditing>(
        GlossaryService.connectionUrl + "edit/"+glossaryId,
        glossaryId
      )
      .pipe(catchError(this.handleError("getGlossaryForEditing", null)))
      .toPromise();
    return result;
  }

  /**
   * Добавление нового словарь.
   * @param glossary Новый словарь.
   */
  addNewGlossary(glossary: GlossariesForEditing): Observable<Object> {
    return this.httpClient.post(
      GlossaryService.connectionUrl + "newGlossary",
      glossary
    );
  }

  /**
   * Сохранение изменений в словарь.
   * @param glossary Отредактированный словарь.
   */
  editSaveGlossary(glossary: GlossariesForEditing): Observable<Object> {
    return this.httpClient.post(
      GlossaryService.connectionUrl + "editSaveGlossary",
      glossary
    );
  }

  /**
   * Удаление словарь.
   * @param glossaryId Идентификатор словарь.
   */
  deleteGlossary(glossaryId: Guid): Observable<Object> {
    return this.httpClient.delete(
      GlossaryService.connectionUrl + "deleteGlossary/" + glossaryId
    );
  }

  //
  /**
   * Удаление всех терминов словарь.
   * @param glossaryId Идентификатор словарь.
   */
  clearGlossary(glossaryId: Guid): Observable<Object> {
    return this.httpClient.delete(
      GlossaryService.connectionUrl + "clearGlossary/" + glossaryId
    );
  }

  /**
   * Обработка исключений (вывод на консоль).
   * @param operation Наименование функции/метода, в которой(ом) перехвачено исключение.
   * @param result Результат функции/метода.
   */
  handleError<T>(operation = "Operation", result?: T) {
    return (error: any): Observable<T> => {
      console.log(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }
}
