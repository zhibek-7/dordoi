import { Locale } from 'src/app/models/database-entities/locale.type';
import { localizationProjectForSelectDTO } from 'src/app/models/DTO/localizationProjectForSelectDTO.type';

export class GlossariesForEditing {
  id: number;
  name: string;
  description: string;

  locales: Locale[];

  localizationProjects: localizationProjectForSelectDTO[];
}

export class GlossariesTableViewDTO {
  id: number;
  name: string;

  localesName: string;

  localizationProjectsName: string;
}
