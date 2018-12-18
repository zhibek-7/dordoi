export class TranslatedWordsReportRow {
    name?: string;
    language?: string;
    workType?: string;
    translations?: string;
    

  constructor(name: string, language: string, workType: string, translations: string)
    {
        this.name = name;
        this.language = language;
        this.translations = translations;
        this.workType = workType;
    }
}
