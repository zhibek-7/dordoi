import { Guid } from 'guid-typescript';

export class FileViewModel {
  constructor(
    public name_text: string,
    public id: Guid
  ) { }
}
