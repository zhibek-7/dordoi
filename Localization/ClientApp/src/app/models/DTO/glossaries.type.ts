export class Glossaries {
  id: number;
  name: string;
  //description: string;

  localeId : number;
  localeName: string;
  localizationProjectId: number;
  localizationProjectName: string;

  //Locales: [id: number, name: string];
  //LocalizationProjects: [number, string];
}

export class GlossariesDTO {
  id: number;
  name: string;
  //description: string;
  
  localesName: string;
  localizationProjectsName: string;
  
}
