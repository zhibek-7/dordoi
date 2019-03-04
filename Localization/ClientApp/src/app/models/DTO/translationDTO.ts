/** Для редактирования поля: translations.translated */
export class TranslationDTO {
  constructor(
    public id: number,
    public translated: string,
    public locale_name: string
  ) { }
}
