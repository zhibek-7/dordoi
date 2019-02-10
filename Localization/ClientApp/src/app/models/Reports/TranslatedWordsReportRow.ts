export class TranslatedWordsReportRow {
  name_text?: string;
  language?: string;
  work_Type?: string;
  translations?: string;


  constructor(name: string, language: string, workType: string, translations: string) {
    this.name_text = name;
    this.language = language;
    this.translations = translations;
    this.work_Type = workType;
  }
}
