import { Guid } from 'guid-typescript';

export class Locale {
  id: Guid;
  name_text: string;
  description: string;
  flag: string;
  isNative?: boolean;
  url: string;
  public constructor(
    id: Guid,
    name: string,
    description: string,
    flag: string,
    url: string = null
  ) {
    this.id = id;
    this.name_text = name;
    this.description = description;
    this.flag = flag;
    this.url = url;
  }
}
