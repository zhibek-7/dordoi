export class TranslatedWordsReportRow {
    name?: string;
    language?: string;
    translations?: string;
    confirmed?: string;

    constructor(name: string, language: string, translations: string, confirmed: string)
    {
        this.name = name;
        this.language = language;
        this.translations = translations;
        this.confirmed = confirmed;
    }
}
