export class Project {

  name?: string;
  ableToDownload?: boolean;
  ableToLeftErrors?: boolean;
  dateOfCreation?: Date;
  defaultString ?: string ;
  description?: string;
  ID_SourceLocale?: number;
  lastActivity?: Date;
  logo?: string;
  notifyConfirm?: boolean;
  notifyFinish?: boolean;
  notifyNew?: boolean;
  visibility?: boolean;
  id?: number;
  url ?: string;


  constructor(name: string, ableToDownload: boolean, ableToLeftErrors: boolean, dateOfCreation: Date, defaultString: string, description: string,
    ID_SourceLocale: number, lastActivity: Date, logo: string, notifyConfirm: boolean, notifyFinish: boolean, notifyNew: boolean, visibility: boolean, id: number, url: string)
    {
      this.name = name;
      this.ableToDownload = ableToDownload;
      this.ableToLeftErrors = ableToLeftErrors;
      this.dateOfCreation = dateOfCreation;
      this.defaultString = defaultString;
      this.description = description;
      this.ID_SourceLocale = ID_SourceLocale;
      this.lastActivity = lastActivity;
      this.logo = logo;
      this.notifyConfirm = notifyConfirm;
      this.notifyFinish = notifyFinish;
      this.notifyNew = notifyNew;
      this.visibility = visibility;
      this.id = id;
      this.url = url;
   }
}
