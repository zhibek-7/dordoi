export class TranslationSubstring {

  get id(): number {
    return this.iD;
  }

  get id_fileOwner(): number {
    return this.iD_fileOwner;
  }

  constructor(
    public iD?: number,
    public substringToTranslate?: string,
    public description?: string,
    public context?: string,
    public iD_fileOwner?: number,
    public translationMaxLength?: number,
    public value?: string,
    public positionInText?: number,
    public outdated?: boolean
  ) { }

}
