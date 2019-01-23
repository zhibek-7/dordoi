//import { Locale } from 'src/app/models/database-entities/locale.type';
//import { localizationProjectForSelectDTO } from 'src/app/models/DTO/localizationProjectForSelectDTO.type';

export class GlossariesForEditing {
  id: number;
  name: string;
  description: string;

  localesIds: number[];

  localizationProjectsIds: number[];
}

export class GlossariesTableViewDTO {
  id: number;
  name: string;
  //id_file?: number;
  //description: string;

  localesName: string;

  localizationProjectsName: string;
}
