import { Guid } from "guid-typescript";

export class Translation {
  public id?: Guid;
  public iD_String?: Guid;
  public translated?: string;
  public confirmed?: boolean = false;
  public iD_User?: Guid;
  public user_Name?: string;
  public dateTime?: Date;
  public iD_Locale?: Guid;
  public selected?: boolean = false;

  constructor(translated: string, id_string: any, id_locale: any, id?: any) {
    this.id = id;
    this.iD_String = id_string;
    this.translated = translated;
    this.confirmed = false;
    this.dateTime = new Date(Date.now());
    this.iD_Locale = id_locale;
  }
  
}
