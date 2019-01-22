export class TranslationSubstring {

  // get id(): number {
  //   return this.iD;
  // }

  get id_fileOwner(): number {
    return this.iD_FileOwner;
  }

  constructor(
    public id?: number,
    public substringToTranslate?: string,
    public description?: string,
    public context?: string,
    public iD_FileOwner?: number,
    public translationMaxLength?: number,
    public value?: string,
    public positionInText?: number,
    public outdated?: boolean
  ) { }

}
