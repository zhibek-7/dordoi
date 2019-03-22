import { Guid } from 'guid-typescript';

/** Для редактирования поля: translations.translated */
export class TranslationDTO {
  constructor(
    public id: Guid,
    public translated: string,
    public locale_name: string
  ) { }
}
