import { Guid } from "guid-typescript";

/**
 * Тип услуг, содержащий только идентификатор и наименование.
 * Для выборки, например checkbox. */
export class TranslationTopicForSelectDTO {
  id: Guid;
  name_text: string;
}
