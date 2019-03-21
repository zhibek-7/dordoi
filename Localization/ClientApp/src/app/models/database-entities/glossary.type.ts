import { Guid } from 'guid-typescript';

export class Glossary {
  public constructor(
    public id?: Guid,
    public name_text?: string,
    public description?: string
  ) { }
}
