import { Guid } from 'guid-typescript';

/***
 * Язык перевода назначенный на проект локализации с процентом перевода
 */
export class LocalizationProjectsLocalesDTO {
  // Идентификатор языка
  locale_Id: Guid;
  // Наименование языка
  locale_Name: string;
  // Путь к изображению флага
  locale_Url: string;

  // Процент переведенных слов
  percent_Of_Translation: number;
  // Процент подтвержденных переводов
  percent_Of_Confirmed: number;
}
