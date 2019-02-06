export class TranslationSubstring {
  // get id(): number {
  //   return this.iD;
  // }

  get id_fileOwner(): number {
    return this.iD_File_Owner;
  }

  constructor(
    public id?: number,
    public substring_To_Translate?: string,
    public description?: string,
    public context?: string,
    public iD_File_Owner?: number,
    public translation_Max_Length?: number,
    public value?: string,
    public position_In_Text?: number,
    public outdated?: boolean
  ) {}
}
