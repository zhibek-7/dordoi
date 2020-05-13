import { Guid } from 'guid-typescript';

export class Project {

  name_text?: string;
  able_To_Download?: boolean;
  able_To_Left_Errors?: boolean;
  date_Of_Creation?: Date;
  default_String?: string;
  description?: string;
  iD_Source_Locale?: Guid;
  last_Activity?: Date;
  logo?: string;
  notify_Confirm?: boolean;
  notify_Finish?: boolean;
  notify_New?: boolean;
  visibility?: boolean;
  id?: Guid;
  url?: string;


  constructor(name: string, ableToDownload: boolean, ableToLeftErrors: boolean, dateOfCreation: Date, defaultString: string, description: string,
    iD_SourceLocale: Guid, lastActivity: Date, logo: string, notifyConfirm: boolean, notifyFinish: boolean, notifyNew: boolean, visibility: boolean, id: Guid, url: string) {
    this.name_text = name;
    this.able_To_Download = ableToDownload;
    this.able_To_Left_Errors = ableToLeftErrors;
    this.date_Of_Creation = dateOfCreation;
    this.default_String = defaultString;
    this.description = description;
    this.iD_Source_Locale = iD_SourceLocale;
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
