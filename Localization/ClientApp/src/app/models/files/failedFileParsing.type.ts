export class FailedFileParsingModel {
  constructor(
    public fileName: string,
    public parserMessage: string,
  ) { }
}
