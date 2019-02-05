// Модель проектов
export class LocalizationProjectsLocales {
  id_LocalizationProject?: number;
  id_Locale?: number;
  percentOfTranslation: number = 0xf00d;
  PercentOfConfirmed: number = 0xf00d;

  public constructor(id_LocalizationProject: number, id_Locale: number, percentOfTranslation: number = 0xf00d, PercentOfConfirmed: number = 0xf00d) {
    this.id_LocalizationProject = id_LocalizationProject;
    this.id_Locale = id_Locale;
    this.percentOfTranslation = percentOfTranslation;
    this.PercentOfConfirmed = PercentOfConfirmed;
  }
}
