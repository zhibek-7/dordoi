import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Language } from '../models/Language';
import { Observable, of } from 'rxjs';

@Injectable()
export class LanguageService {

    public languages: Language[]  = [
        {
          name: 'English, United Kingdom',
          description: 'English, United Kingdom',
          flag: 'English, United Kingdom'
        },
        {
          name: 'English, United States',
          description: 'English, United States',
          flag: 'English, United States'
        },
        {
          name: 'Dutch, Suriname',
          description: 'Dutch, Suriname',
          flag: 'Dutch, Suriname'
        },
        {
          name: 'Russian',
          description: 'Russian',
          flag: 'Russian'
        }
  ];

  private url: String = "api/Language/";

    constructor(private httpClient: HttpClient) { }

    getLanguage(userName: string) {
        const getUrl = ''; // тут формируем строку подключения к БД для получения языков
        return this.httpClient.get(getUrl);
    }

  getLanguageList(): Observable<Language[]> {
    return this.httpClient.get<Language[]>(this.url + "List");
  }

}
