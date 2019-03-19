import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
//import { Translator } from 'src/app/models/Translators/translator.type';

@Injectable()
export class TranslatorsService {
  constructor() {}

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
            user_Id: 187,
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
          {
            user_Id: 188,
            Profile: true,
            user_Name: 'Ivan Petrov',
            translationRating: 100,
            termRating: 90,
            topics: ['PR', 'Машиностроение', 'ИТ'],
            languages: ['Русский', 'Английский', 'Французский'],
            wordsQuantity: 45.693,
            service: 'Редактура',
            cost: 3.83
          },
          {
            user_Id: 189,
            Profile: true,
            user_Name: 'Ivan Petrov',
            translationRating: 100,
            termRating: 90,
            topics: ['PR', 'Машиностроение', 'ИТ'],
            languages: ['Русский', 'Английский', 'Французский'],
            wordsQuantity: 45.693,
            service: 'Редактура',
            cost: 3.82
          },
          {
            user_Id: 190,
            Profile: true,
            user_Name: 'Ivan Petrov',
            translationRating: 100,
            termRating: 90,
            topics: ['PR', 'Машиностроение', 'ИТ'],
            languages: ['Русский', 'Английский', 'Французский'],
            wordsQuantity: 45.693,
            service: 'Редактура',
            cost: 3.79
          },
          {
            user_Id: 191,
            Profile: true,
            user_Name: 'Ivan Petrov',
            translationRating: 100,
            termRating: 90,
            topics: ['PR', 'Машиностроение', 'ИТ'],
            languages: ['Русский', 'Английский', 'Французский'],
            wordsQuantity: 45.693,
            service: 'Редактура',
            cost: 3.78
          },
          {
            user_Id: 192,
            Profile: true,
            user_Name: 'Ivan Petrov',
            translationRating: 100,
            termRating: 90,
            topics: ['PR', 'Машиностроение', 'ИТ'],
            languages: ['Русский', 'Английский', 'Французский'],
            wordsQuantity: 45.693,
            service: 'Редактура',
            cost: 3.77
          },
          {
            user_Id: 193,
            Profile: true,
            user_Name: 'Ivan Petrov',
            translationRating: 100,
            termRating: 90,
            topics: ['PR', 'Машиностроение', 'ИТ'],
            languages: ['Русский', 'Английский', 'Французский'],
            wordsQuantity: 45.693,
            service: 'Редактура',
            cost: 3.76
          },
          {
            user_Id: 194,
            Profile: true,
            user_Name: 'Ivan Petrov',
            translationRating: 100,
            termRating: 90,
            topics: ['PR', 'Машиностроение', 'ИТ'],
            languages: ['Русский', 'Английский', 'Французский'],
            wordsQuantity: 45.693,
            service: 'Редактура',
            cost: 3.75
          },
          {
            user_Id: 195,
            Profile: true,
            user_Name: 'Ivan Petrov',
            translationRating: 100,
            termRating: 90,
            topics: ['PR', 'Машиностроение', 'ИТ'],
            languages: ['Русский', 'Английский', 'Французский'],
            wordsQuantity: 45.693,
            service: 'Редактура',
            cost: 3.74
          },
          {
            user_Id: 196,
            Profile: true,
            user_Name: 'Ivan Petrov',
            translationRating: 100,
            termRating: 90,
            topics: ['PR', 'Машиностроение', 'ИТ'],
            languages: ['Русский', 'Английский', 'Французский'],
            wordsQuantity: 45.693,
            service: 'Редактура',
            cost: 3.73
          },
          {
            user_Id: 197,
            Profile: true,
            user_Name: 'Ivan Petrov',
            translationRating: 100,
            termRating: 90,
            topics: ['PR', 'Машиностроение', 'ИТ'],
            languages: ['Русский', 'Английский', 'Французский'],
            wordsQuantity: 45.693,
            service: 'Редактура',
            cost: 3.72
          },
          {
            user_Id: 198,
            Profile: true,
            user_Name: 'Ivan Petrov',
            translationRating: 100,
            termRating: 90,
            topics: ['PR', 'Машиностроение', 'ИТ'],
            languages: ['Русский', 'Английский', 'Французский'],
            wordsQuantity: 45.693,
            service: 'Редактура',
            cost: 3.71
          },
          {
            user_Id: 199,
            Profile: true,
            user_Name: 'Ivan Petrov',
            translationRating: 100,
            termRating: 90,
            topics: ['PR', 'Машиностроение', 'ИТ'],
            languages: ['Русский', 'Английский', 'Французский'],
            wordsQuantity: 45.693,
            service: 'Редактура',
            cost: 3.70
          },
        ]
      );
      observer.complete();
    });
    return translators;
  }
}
