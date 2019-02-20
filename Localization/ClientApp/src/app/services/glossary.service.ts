import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders  } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { GlossariesForEditing, GlossariesTableViewDTO } from 'src/app/models/DTO/glossariesDTO.type';
import { catchError } from 'rxjs/operators';


@Injectable()
export class GlossaryService
{
  static connectionUrl: string = 'api/glossaries/';

  constructor(private httpClient: HttpClient) { }


  /**
   * Возвращает список глоссариев, со строками перечислений имен связанных объектов.
   */
  getGlossariesDTO(): Observable<GlossariesTableViewDTO[]>
  {
    return this.httpClient.post<GlossariesTableViewDTO[]>(GlossaryService.connectionUrl, null, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    });
  }  
  
  /**
   * Добавление нового глоссария.
   * @param glossary Новый глоссарий.
   */
  addNewGlossary(glossary: GlossariesForEditing): Observable<Object>
  {
    return this.httpClient.post(GlossaryService.connectionUrl + "newGlossary", glossary, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    });
  }
  
  /**
   * Возвращает глоссарий для редактирования (со связанными объектами).
   * @param glossaryId Идентификатор глоссария.
   */
  async getGlossaryForEditing(glossaryId: number): Promise<GlossariesForEditing>
  {
    let result = await this.httpClient.post<GlossariesForEditing>(GlossaryService.connectionUrl + "edit", glossaryId, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    })
      .pipe(catchError(this.handleError('getGlossaryForEditing', null))).toPromise();
    return result;
  }
  
  /**
   * Сохранение изменений в глоссарии.
   * @param glossary Отредактированный глоссарий.
   */
  editSaveGlossary(glossary: GlossariesForEditing): Observable<Object>
  {
    return this.httpClient.post(GlossaryService.connectionUrl + "editSaveGlossary", glossary, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    });
  }
  
  /**
   * Удаление глоссария.
   * @param glossaryId Идентификатор глоссария.
   */
  deleteGlossary(glossaryId: number): Observable<Object>
  {
    return this.httpClient.delete(GlossaryService.connectionUrl + "deleteGlossary/" + glossaryId, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    });
  }

  // 
  /**
   * Удаление всех терминов глоссария.
   * @param glossaryId Идентификатор глоссария.
   */
  clearGlossary(glossaryId: number): Observable<Object>
  {
    return this.httpClient.delete(GlossaryService.connectionUrl + "clearGlossary/" + glossaryId, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    });
  }


  /**
   * Обработка исключений (вывод на консоль).
   * @param operation Наименование функции/метода, в которой(ом) перехвачено исключение.
   * @param result Результат функции/метода.
   */
  handleError<T>(operation = 'Operation', result?: T)
  {
    return (error: any): Observable<T> => {
      console.log(`${operation} failed: ${error.message}`);
      return of(result as T);
    }
  }
}
