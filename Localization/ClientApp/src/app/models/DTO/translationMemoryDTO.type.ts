/**
 * Для создания новой и редактирования памяти переводов
 */
export class TranslationMemoryForEditingDTO {
  id: number;
  name_text: string;

  locales_ids: number[];

  localization_projects_ids: number[];
}

/**
 * Для отображения в таблице списка памяти переводов
 */
export class TranslationMemoryTableViewDTO {
  id: number;
  name_text: string;

  string_count: number;

  locales_name: string;

  localization_projects_name: string;
}

export class TranslationMemoryForSelectDTO {
  id: number;
  name_text: string;
}
