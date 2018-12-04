import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Language } from '../models/Language';

@Injectable()
export class LanguageService {
    languages: Language[]  = [
        {
          name: 'English, United Kingdom'
        },
        {
          name: 'English, United States'
        },
        {
          name: 'Dutch, Suriname'
        },
        {
          name: 'Russian'
        }
      ];

    constructor(private httpClient: HttpClient) { }

    getLanguage(userName: string) {
        const getUrl = ''; // тут формируем строку подключения к БД для получения языков
        return this.httpClient.get(getUrl);
    }

}
