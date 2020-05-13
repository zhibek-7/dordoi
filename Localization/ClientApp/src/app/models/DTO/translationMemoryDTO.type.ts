import { Guid } from "guid-typescript";

/**
 * Для создания новой и редактирования памяти переводов
 */
export class TranslationMemoryForEditingDTO {
  id: Guid;
  id_file: Guid;
  name_text: string;
  locales_ids: Guid[];

  localization_projects_ids: Guid[];
}

/**
 * Для отображения в таблице списка памяти переводов
 */
export class TranslationMemoryTableViewDTO {
  id: Guid;
  name_text: string;

  string_count: number;

  locales_name: string;

  localization_projects_name: string;
}

export class TranslationMemoryForSelectDTO {
  id: Guid;
  name_text: string;
}
