export class TranslationSubstring {
  // get id(): number {
  //   return this.iD;
  // }
  /*id?: number;
  substring_to_translate?: string;
  description?: string;
  context?: string;
  id_file_owner?: number;
  translation_max_length?: number;
  value?: string;
  position_In_Text?: number;
  outdated?: boolean;*/

  get id_fileOwner(): number {
    return this.id_file_owner;
  }

  constructor(
    public id?: number,
    public substring_to_translate?: string,
    public description?: string,
    public context?: string,
    public id_file_owner?: number,
    public translation_max_length?: number,
    public value?: string,
    public position_In_Text?: number,
    public outdated?: boolean,
    public status?: string
  ) {}
}
