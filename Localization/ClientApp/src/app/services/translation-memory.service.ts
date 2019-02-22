import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { TranslationMemoryForEditingDTO, TranslationMemoryTableViewDTO } from 'src/app/models/DTO/translationMemoryDTO.type';
import { catchError } from 'rxjs/operators';


@Injectable()
export class TranslationMemoryService {
  static connectionUrl: string = 'api/TranslationMemory/';

  constructor(private httpClient: HttpClient) { }


  /**
   * Возвращает список памяти переводов, со строками перечислений имен связанных объектов.
   */
  getAllDTO(): Observable<TranslationMemoryTableViewDTO[]> {
    return this.httpClient.post<TranslationMemoryTableViewDTO[]>(TranslationMemoryService.connectionUrl, null, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }

  /**
   * Добавление новой памяти переводов.
   * @param translationMemory Новая память переводов.
   */
  create(translationMemory: TranslationMemoryForEditingDTO): Observable<Object> {
    return this.httpClient.post(TranslationMemoryService.connectionUrl + "create", translationMemory, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }

  /**
   * Возвращает память переводов для редактирования (со связанными объектами).
   * @param id Идентификатор памяти переводов.
   */
  async getForEditing(id: number): Promise<TranslationMemoryForEditingDTO> {
    let result = await this.httpClient.post<TranslationMemoryForEditingDTO>(TranslationMemoryService.connectionUrl + "edit", id, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    })
      .pipe(catchError(this.handleError('getForEditing', null))).toPromise();
    return result;
  }

  /**
   * Сохранение изменений в памяти переводов.
   * @param translationMemory Отредактированная память переводов.
   */
  editSave(translationMemory: TranslationMemoryForEditingDTO): Observable<Object> {
    return this.httpClient.post(TranslationMemoryService.connectionUrl + "editSave", translationMemory, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }

  /**
   * Удаление памяти переводов.
   * @param id Идентификатор памяти переводов.
   */
  delete(id: number): Observable<Object> {
    return this.httpClient.delete(TranslationMemoryService.connectionUrl + "delete/" + id, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }

  // 
  /**
   * Удаление всех строк памяти переводов.
   * @param id Идентификатор памяти переводов.
   */
  clear(id: number): Observable<Object> {
    return this.httpClient.delete(TranslationMemoryService.connectionUrl + "clear/" + id, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }


  /**
   * Обработка исключений (вывод на консоль).
   * @param operation Наименование функции/метода, в которой(ом) перехвачено исключение.
   * @param result Результат функции/метода.
   */
  handleError<T>(operation = 'Operation', result?: T) {
    return (error: any): Observable<T> => {
      console.log(`${operation} failed: ${error.message}`);
      return of(result as T);
    }
  }
}
