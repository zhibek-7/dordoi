export class TranslationSubstring {

  constructor(
    public id?: number,
    public substringToTranslate?: string,
    public description?: string,
    public context?: string,
    public id_fileOwner?: number,
    public translationMaxLength?: number,
    public value?: string,
    public positionInText?: number,
  ) { }

}
