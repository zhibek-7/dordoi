import { Guid } from "guid-typescript";

export class Translation {
  public id?: Guid;
  public ID_String?: Guid;
  public translated?: string;
  public Confirmed?: boolean = false;
  public ID_User?: Guid;
  public user_Name?: string;
  public dateTime?: Date;
  public ID_Locale?: Guid;
  public Selected?: boolean = false;

  constructor(translated: string, id_string: any, id_locale: any, id?: any) {
    this.id = id;
    this.ID_String = id_string;
    this.translated = translated;
    this.Confirmed = false;
    this.dateTime = new Date(Date.now());
    this.ID_Locale = id_locale;
  }
}
