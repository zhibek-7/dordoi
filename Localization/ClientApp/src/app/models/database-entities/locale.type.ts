export class Locale {
  id: number;
  name_text: string;
  description: string;
  flag: string;
  isNative?: boolean;
  url: string;
  public constructor(
    id: number,
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
