import { Guid } from 'guid-typescript';

export class Translator {
  constructor(
    public id?: Guid,
    //public publicProfile?: boolean, // публичный или скрытый аккаунт
    public user_name?: string,
    public user_email?: string,
    //public service?: string, // услуга (перевод, редактура)
    public user_pic?: any,
    public translation_rating?: number, // рейтинг за переводы
    public term_rating?: number, // рейтинг за сроки
    public number_of_ratings?: number, // количество оценок
    public topics?: Array<string>, // темы, в которых у переводчика есть компетенция
    //public languages?: Array<string>, // языки, которыми владеет переводчик
    public words_quantity?: number, // переведено слов
    public cost?: number, // стоимость за слово
    public currency?: string
  ) { }
}
