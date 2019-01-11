import { Locale } from 'src/app/models/database-entities/locale.type';
import { LocalizationProject } from 'src/app/models/DTO/localizationProjectForSelectDTO.type';

export class Glossaries
{
  id: number;
  name: string;

  locales: Locale[];// [number, string];
  //localeId: number;
  //localeName: string;

  localizationProjects: LocalizationProject[];// [number, string];
  //localizationProjectId: number;
  //localizationProjectName: string;
}

export class GlossariesDTO
{
  id: number;
  name: string;

  localesName: string;

  localizationProjectsName: string;
}
