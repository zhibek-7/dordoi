import { Translation } from "src/app/models/database-entities/translation.type";
import { Locale } from "src/app/models/database-entities/locale.type";

export class TranslationWithLocale extends Translation {
  constructor(translation: Translation, public locale: Locale) {
    super(
      translation.translated,
      translation.iD_String,
      translation.iD_Locale,
      translation.id
    );
  }
}
