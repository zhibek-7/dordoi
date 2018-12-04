import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { PartOfSpeech } from 'src/app/models/database-entities/partOfSpeech.type';

@Injectable()
export class PartsOfSpeechService {

  static connectionUrl: string = 'api/partsofspeech/';

  constructor(private httpClient: HttpClient) { }

  public getByLocale(locale: string): Observable<PartOfSpeech[]> {
    return this.httpClient.get<PartOfSpeech[]>(PartsOfSpeechService.connectionUrl + locale);
  }

}
