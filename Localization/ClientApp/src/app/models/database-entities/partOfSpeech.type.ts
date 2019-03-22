import { Guid } from 'guid-typescript';

export class PartOfSpeech {
  public constructor(
    public id: Guid,
    public locale_Id: Guid,
    public name_text: string
  ) { }
}
