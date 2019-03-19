/**
 * Для создания нового и редактирования глоссария
 */
export class GlossariesForEditing {
  id: number;
  name_text: string;
  description: string;

  locales_Ids: number[];

  localization_Projects_Ids: number[];
}

/**
 * Для отображения в таблице списка глоссарий
 */
export class GlossariesTableViewDTO {
  id: number;
  name: string;

  locales_name: string;

  localization_projects_name: string;
}
