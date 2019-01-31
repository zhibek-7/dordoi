/**
 * Язык перевода назначенный на проект локализации с процентом перевода
 */
export  class LocalizationProjectsLocalesDTO
{
  // Наименование языка
  localeName: string;
  // Путь к изображению флага
  localeUrl: string;

  // Процент переведенных слов
  percentOfTranslation: number;
  // Процент подтвержденных переводов
  percentOfConfirmed: number;
}
