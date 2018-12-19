import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Locale } from '../models/database-entities/locale.type';
import { Observable, of } from 'rxjs';

@Injectable()
export class LanguageService {

  private url: string = "api/Language/";

    constructor(private httpClient: HttpClient) { }

  getLanguageList(): Observable<Locale[]> {
    return this.httpClient.get<Locale[]>(this.url + "List");
  }

}
