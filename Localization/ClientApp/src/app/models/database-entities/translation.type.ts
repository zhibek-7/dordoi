import { Guid } from "guid-typescript";

export class Translation {
  public id?: Guid;
  public ID_String?: Guid;
  public Translated?: string;
  public Confirmed?: boolean = false;
  public ID_User?: Guid;
  public User_Name?: string;
  public DateTime?: Date;
  public ID_Locale?: Guid;
  public Selected?: boolean = false;

  constructor(translated: string, id_string: any, id_locale: any, id?: any) {
    this.id = id;
    this.ID_String = id_string;
    this.Translated = translated;
    this.Confirmed = false;
    this.DateTime = new Date(Date.now());
    this.ID_Locale = id_locale;
  }
}
