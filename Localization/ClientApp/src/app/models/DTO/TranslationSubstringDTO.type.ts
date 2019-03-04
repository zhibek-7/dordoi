import { TranslationDTO } from "./translationDTO";

/**
 * Для отображения в таблице (в разделе текущего проекта)
 */
export class TranslationSubstringTableViewDTO {
  id: number;
  substring_to_translate: string;

  translation_memories_name: string;
}

/** Для редактирования полей: translationSubstrings.substring_to_translate и translations.translated */
export class TranslationSubstringForEditingDTO {

  public constructor(
    public id: number,
    public substring_to_translate: string,
    public localization_project_source_locale_name?: string,

    public translations?: TranslationDTO[]
  ) { }
}
