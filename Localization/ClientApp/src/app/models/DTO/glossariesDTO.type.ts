import { Guid } from 'guid-typescript';

/**
 * Для создания нового и редактирования словарь
 */
export class GlossariesForEditing {
  id: Guid;
  name_text: string;
  description: string;

  locales_Ids: Guid[];

  localization_Projects_Ids: Guid[];
}

/**
 * Для отображения в таблице списка словарь
 */
export class GlossariesTableViewDTO {
  id: Guid;
  name: string;

  locales_name: string;

  localization_projects_name: string;
}
