import { Guid } from 'guid-typescript';

export class FileTranslationInfo {
  public constructor(
    public locale_Id: Guid,
    public percent_Of_Translation: number,
    public percent_Of_Confirmed: number
  ) {}
}
