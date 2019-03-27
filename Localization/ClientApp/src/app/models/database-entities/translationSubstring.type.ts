import { Guid } from "guid-typescript";

export class TranslationSubstring {
  // get id(): number {
  //   return this.iD;
  // }
  /*id?: number;
  substring_to_translate?: string;
  description?: string;
  context?: string;
  id_file_owner?: Guid;
  translation_max_length?: number;
  value?: string;
  position_In_Text?: number;
  outdated?: boolean;*/

  get id_fileOwner(): Guid {
    return this.id_file_owner;
  }

  constructor(
    public id?: Guid,
    public substring_to_translate?: string,
    public description?: string,
    public context?: string,
    public id_file_owner?: Guid,
    public translation_max_length?: number,
    public value?: string,
    public position_In_Text?: number,
    public context_file?: string,
    public outdated?: boolean,
    public status?: string
  ) {
    //console.log("TranslationSubstring.context_file=" + context_file);
  }
}
