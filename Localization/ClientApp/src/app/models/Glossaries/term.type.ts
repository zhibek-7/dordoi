import { TranslationSubstring } from "src/app/models/database-entities/translationSubstring.type";

export class Term extends TranslationSubstring {
  constructor(
    id?: number,
    substring_To_Translate?: string,
    description?: string,
    context?: string,
    id_fileOwner?: number,
    translation_Max_Length?: number,
    value?: string,
    position_In_Text?: number,
    public part_Of_Speech_Id?: number,
    public is_Editable?: boolean
  ) {
    super(
      id,
      substring_To_Translate,
      description,
      context,
      id_fileOwner,
      translation_Max_Length,
      value,
      position_In_Text
    );
  }
}
