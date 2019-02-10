// Модель проектов
export class LocalizationProjectsLocales {
  id_Localization_Project: number;
  id_Locale: number;
  percent_Of_Translation: number = 0xf00d;
  Percent_Of_Confirmed: number = 0xf00d;

  public constructor(
    id_LocalizationProject: number,
    id_Locale: number,
    percentOfTranslation: number = 0xf00d,
    PercentOfConfirmed: number = 0xf00d
  ) {
    this.id_Localization_Project = id_LocalizationProject;
    this.id_Locale = id_Locale;
    this.percent_Of_Translation = percentOfTranslation;
    this.Percent_Of_Confirmed = PercentOfConfirmed;
  }
}
