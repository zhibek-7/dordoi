export class Project {

  name_text?: string;
  able_To_Download?: boolean;
  able_To_Left_Errors?: boolean;
  date_Of_Creation?: Date;
  default_String?: string;
  description?: string;
  ID_Source_Locale?: number;
  last_Activity?: Date;
  logo?: string;
  notify_Confirm?: boolean;
  notify_Finish?: boolean;
  notify_New?: boolean;
  visibility?: boolean;
  id?: number;
  url?: string;


  constructor(name: string, ableToDownload: boolean, ableToLeftErrors: boolean, dateOfCreation: Date, defaultString: string, description: string,
    ID_SourceLocale: number, lastActivity: Date, logo: string, notifyConfirm: boolean, notifyFinish: boolean, notifyNew: boolean, visibility: boolean, id: number, url: string) {
    this.name_text = name;
    this.able_To_Download = ableToDownload;
    this.able_To_Left_Errors = ableToLeftErrors;
    this.date_Of_Creation = dateOfCreation;
    this.default_String = defaultString;
    this.description = description;
    this.ID_Source_Locale = ID_SourceLocale;
    this.last_Activity = lastActivity;
    this.logo = logo;
    this.notify_Confirm = notifyConfirm;
    this.notify_Finish = notifyFinish;
    this.notify_New = notifyNew;
    this.visibility = visibility;
    this.id = id;
    this.url = url;
  }
}
