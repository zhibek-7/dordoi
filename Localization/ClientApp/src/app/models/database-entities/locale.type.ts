export class Locale {
  id: number;
  name_text: string;
  description: string;
  flag: string;
  isNative?: boolean;
  public constructor(
    id: number,
    name: string,
    description: string,
    flag: string
  ) {
    this.id = id;
    this.name_text = name;
    this.description = description;
    this.flag = flag;
  }
}
