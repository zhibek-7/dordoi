import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TypeOfServiceForSelectDTO } from '../models/DTO/typeOfServiceForSelectDTO.type';

@Injectable()
export class TypeOfServiceService {

  private url: string = 'api/TypeOfService/';

  constructor(private httpClient: HttpClient) { }

  /**
   * Возвращает список тип услуг, содержащий только идентификатор и наименование.
   * Для выборки, например checkbox. */
  getAllForSelect(): Observable<TypeOfServiceForSelectDTO[]> {
    return this.httpClient.post<TypeOfServiceForSelectDTO[]>(this.url + "forSelect", null);
  }

}
