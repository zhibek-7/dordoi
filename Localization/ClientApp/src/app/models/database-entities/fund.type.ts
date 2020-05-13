import { Guid } from "guid-typescript";
export class fund {
  public id: Guid;
  public ID_User: string;
  public name_text: string;
  public description: string;
  public date_time_added: Date;


  public constructor(name_text: string, description: string) {
    this.ID_User = null; //Guid.createEmpty()
    this.name_text = name_text;
    this.description = description;
    this.date_time_added = null;
  }
}
