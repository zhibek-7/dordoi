import { Guid } from 'guid-typescript';

// Модель проектов
export class LocalizationProjectsLocales {
  id_Localization_Project: Guid;
  id_Locale: Guid;
  percent_Of_Translation: number = 0;
  Percent_Of_Confirmed: number = 0;

  public constructor(
    id_LocalizationProject: Guid,
    id_Locale: Guid,
    percentOfTranslation: number = 0,
    PercentOfConfirmed: number = 0
  ) {
    this.id_Localization_Project = id_LocalizationProject;
    this.id_Locale = id_Locale;
    this.percent_Of_Translation = percentOfTranslation;
    this.Percent_Of_Confirmed = PercentOfConfirmed;
  }
}
