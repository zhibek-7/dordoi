import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TranslationTopicForSelectDTO } from '../models/DTO/translationTopicForSelectDTO.type';

@Injectable()
export class TranslationTopicService {

  private url: string = 'api/TranslationTopic/';

  constructor(private httpClient: HttpClient) { }

  /**
   * Возвращает список тематик, содержащий только идентификатор и наименование.
   * Для выборки, например checkbox. */
  getAllForSelect(): Observable<TranslationTopicForSelectDTO[]> {
    return this.httpClient.post<TranslationTopicForSelectDTO[]>(this.url + "forSelect", null);
  }

}
