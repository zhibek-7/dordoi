import { String } from "src/app/models/database-entities/string.type";

export class Term extends String {
  constructor(
    id?: number,
    substringToTranslate?: string,
    description?: string,
    context?: string,
    id_fileOwner?: number,
    translationMaxLength?: number,
    value?: string,
    positionInText?: number,
    public partOfSpeechId?: number,
    public isEditable?: boolean,
  ) {
      super(
        id,
        substringToTranslate,
        description,
        context,
        id_fileOwner,
        translationMaxLength,
        value,
        positionInText);
  }
}
