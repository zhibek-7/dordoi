export class Locale {
  id: number;
  name_text: string;
  description: string;
  flag: string;
  public constructor(id: number,
    name: string,
    description: string,
    flag: string

  ) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.flag = flag;
  }


}
