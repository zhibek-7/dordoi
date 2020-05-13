import { Guid } from 'guid-typescript';

// Модель проектов
export class LocalizationProjectsLocales {
  iD_Localization_Project: Guid;
  iD_Locale: Guid;
  percent_Of_Translation: number = 0;
  percent_Of_Confirmed: number = 0;

  public constructor(
    id_LocalizationProject: Guid,
    id_Locale: Guid,
    percentOfTranslation: number = 0,
    PercentOfConfirmed: number = 0
  ) {
    this.iD_Localization_Project = id_LocalizationProject;
    this.iD_Locale = id_Locale;
    this.percent_Of_Translation = percentOfTranslation;
    this.percent_Of_Confirmed = PercentOfConfirmed;
  }
}
