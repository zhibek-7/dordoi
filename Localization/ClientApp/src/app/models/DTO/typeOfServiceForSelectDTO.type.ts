import { Guid } from "guid-typescript";

/**
 * Тематика, содержащая только идентификатор и наименование.
 * Для выборки, например checkbox. */
export class TypeOfServiceForSelectDTO {
  id: Guid;
  name_text: string;
}
