import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpParams,
  HttpResponse,
  HttpHeaders
} from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { Translator } from 'src/app/models/Translators/translator.type';

@Injectable()
export class TranslatorsService {
  constructor(private httpClient: HttpClient) {}

  getAllPublicTranslators() {

    let translators = Observable.create(function(observer) {
      observer.next(
        [
          {
            user_Id: 156,
            Profile: true,
            user_Name: 'Vladimir Ivanov',
            translationRating: 95,
            termRating: 99,
            topics: ['Маркетинг', 'Машиностроение'],
            languages: ['Русский', 'Английский'],
            wordsQuantity: 25.693,
            service: 'Перевод',
            cost: 5.689
          },
          {
            user_Id: 186,
            Profile: true,
            user_Name: 'Roman Oletskiy',
            translationRating: 100,
            termRating: 90,
            topics: ['PR', 'Машиностроение', 'ИТ', 'Финансы'],
            languages: ['Русский', 'Английский', 'Испанский'],
            wordsQuantity: 325.693,
            service: 'Перевод',
            cost: 1.68
          },
          {
            user_Id: 186,
            Profile: true,
            user_Name: 'Ivan Petrov',
            translationRating: 100,
            termRating: 90,
            topics: ['PR', 'Машиностроение', 'ИТ'],
            languages: ['Русский', 'Английский', 'Французский'],
            wordsQuantity: 45.693,
            service: 'Редактура',
            cost: 3.68
          },
        ]
      );
      observer.complete();
    });
    return translators;
  }
}
