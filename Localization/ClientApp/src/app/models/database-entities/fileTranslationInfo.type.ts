export class FileTranslationInfo {
  public constructor(
    public localeId: number,
    public percentOfTranslation: number,
    public percentOfConfirmed: number,
  ) { }
}
