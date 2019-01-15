export class Glossaries
{
  id: number;
  name: string;

  localeId: number;
  localeName: string;

  localizationProjectId: number;
  localizationProjectName: string;
}

export class GlossariesDTO
{
  id: number;
  name: string;

  localesName: string;

  localizationProjectsName: string;
}
