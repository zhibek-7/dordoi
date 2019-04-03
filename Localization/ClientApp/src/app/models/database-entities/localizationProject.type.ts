import { Guid } from 'guid-typescript';

/** Модель проектов */
export class LocalizationProject {
  id?: Guid;
  name_text: string;
  description: string;
  url: string;
  visibility: boolean;
  date_Of_Creation?: any;
  last_Activity?: any;
  iD_Source_Locale?: Guid;
  //Исходный язык
  source_Locale_Name: string;
  // Количество активных пользователей
  count_users_active: number;
  able_To_Download: boolean;
  able_To_Left_Errors: boolean;
  default_String: string;
  notify_New: boolean;
  notify_Finish: boolean;
  notify_Confirm: boolean;
  logo?: any;
  notify_new_comment: boolean;
  export_only_approved_translations: boolean;
  original_if_string_is_not_translated: boolean;
  able_translators_change_terms_in_glossaries: boolean;
  //public dateTime: Date = new Date(Date.now()),
  //public id?: number
  public constructor(
    id: Guid,
    name: string,
    description: string,
    url: string,
    visibility: boolean,
    dateOfCreation: any,
    // lastActivity: "12.12.2018",
    id_SourceLocale: Guid,
    ableToDownload: boolean,
    ableToLeftErrors: boolean,
    //defaultString: string,
    notifyNew: boolean,
    notifyFinish: boolean,
    notifyConfirm: boolean,
    notifynewcomment: boolean,
    export_only_approved_translations: boolean,
    original_if_string_is_not_translated: boolean,
    able_translators_change_terms_in_glossaries: boolean
  ) {
    this.id = id;
    this.name_text = name;
    this.description = description;
    this.url = url;
    this.visibility = visibility;
    this.date_Of_Creation = dateOfCreation;
    this.last_Activity = dateOfCreation; // lastActivity;//"12.12.2018";
    this.iD_Source_Locale = id_SourceLocale;
    this.able_To_Download = ableToDownload;
    this.able_To_Left_Errors = ableToLeftErrors;
    // this.defaultString = defaultString;// '123';
    this.notify_New = notifyNew;
    this.notify_Finish = notifyFinish;
    this.notify_Confirm = notifyConfirm;
    this.logo = [];
    this.notify_new_comment = notifynewcomment;
    this.export_only_approved_translations = export_only_approved_translations;
    this.original_if_string_is_not_translated = original_if_string_is_not_translated;
    this.able_translators_change_terms_in_glossaries = able_translators_change_terms_in_glossaries;
  }
}
