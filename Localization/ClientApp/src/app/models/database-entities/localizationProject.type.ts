// Модель проектов
export class LocalizationProject {
  id?: number;
  name: string;
  description: string;
  url: string;
  visibility: boolean;
  dateOfCreation?: any;
  lastActivity?: any;
  ID_SourceLocale?: number;
  ableToDownload: boolean;
  ableToLeftErrors: boolean;
  defaultString: string;
  notifyNew: boolean;
  notifyFinish: boolean;
  notifyConfirm: boolean;
  logo?: any;
  notifynewcomment: boolean;
  export_only_approved_translations: boolean;
  original_if_string_is_not_translated: boolean;
 //public dateTime: Date = new Date(Date.now()),
 //public id?: number
  public constructor(
    id: number,
    name: string,
    description: string,
   url: string,
    visibility: boolean,
    dateOfCreation: any,
   // lastActivity: "12.12.2018",
    id_SourceLocale: number,
    ableToDownload: boolean,
    ableToLeftErrors: boolean,
    //defaultString: string,
    notifyNew: boolean,
    notifyFinish: boolean,
    notifyConfirm: boolean,
    notifynewcomment: boolean,
    export_only_approved_translations: boolean,
    original_if_string_is_not_translated: boolean

  ) {
            this.id = id;
            this.name = name;
            this.description = description;
            this.url = url;
    this.visibility = visibility;
    this.dateOfCreation = dateOfCreation;
    this.lastActivity = "12.12.2018";// lastActivity;//"12.12.2018";
    this.ID_SourceLocale = id_SourceLocale;
    this.ableToDownload = ableToDownload;
    this.ableToLeftErrors = ableToLeftErrors;
   // this.defaultString = defaultString;// '123';
    this.notifyNew = notifyNew;
    this.notifyFinish = notifyFinish;
    this.notifyConfirm = notifyConfirm;
            this.logo = [];
    this.notifynewcomment = notifynewcomment;
    this.export_only_approved_translations = export_only_approved_translations;
    this.original_if_string_is_not_translated = original_if_string_is_not_translated;
   }
}
