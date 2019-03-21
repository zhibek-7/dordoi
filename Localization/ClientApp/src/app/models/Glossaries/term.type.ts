import { TranslationSubstring } from "src/app/models/database-entities/translationSubstring.type";
import { Guid } from 'guid-typescript';

export class Term extends TranslationSubstring {
  constructor(
    id?: Guid,
    substring_to_translate?: string,
    description?: string,
    context?: string,
    id_file_owner?: Guid,
    translation_max_length?: number,
    value?: string,
    position_in_text?: number,
    public part_of_speech_id?: Guid,
    public is_editable?: boolean
  ) {
    super(
      id,
      substring_to_translate,
      description,
      context,
      id_file_owner,
      translation_max_length,
      value,
      position_in_text
    );
  }
}
