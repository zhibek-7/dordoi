export class TranslationSubstring {
  // get id(): number {
  //   return this.iD;
  // }

  get id_fileOwner(): number {
    return this.id_file_owner;
  }

  constructor(
    public id?: number,
    public substring_to_translate?: string,
    public description?: string,
    public context?: string,
    public id_file_owner?: number,
    public translation_Max_Length?: number,
    public value?: string,
    public position_In_Text?: number,
    public outdated?: boolean
  ) {}
}
