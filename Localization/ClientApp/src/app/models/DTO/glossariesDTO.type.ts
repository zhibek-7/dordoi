//Для создания нового и редактирования глоссария
export class GlossariesForEditing {
  id: number;
  name: string;
  description: string;

  localesIds: number[];

  localizationProjectsIds: number[];
}

//Для отображения в таблице списка глоссарий
export class GlossariesTableViewDTO {
  id: number;
  name: string;

  localesName: string;

  localizationProjectsName: string;
}
