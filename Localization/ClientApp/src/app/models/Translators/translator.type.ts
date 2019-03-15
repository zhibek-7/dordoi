export class Translator {
  constructor(
    public user_Id?: number,
    public publicProfile?: boolean, // публичный или скрытый аккаунт
    public user_Name?: string,
    public service?: string, // услуга (перевод, редактура)
    public user_pic?: any,
    public translationRating?: number, // рейтинг за переводы
    public termRating?: number, // рейтинг за сроки
    public topics?: Array<string>, // темы, в которых у переводчика есть компетенция
    public languages?: Array<string>, // языки, которыми владеет переводчик
    public wordsQuantity?: number, // переведено слов
    public cost?: number // стоимость за слово
  ) { }
}
