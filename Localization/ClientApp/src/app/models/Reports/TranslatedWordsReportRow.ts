export class TranslatedWordsReportRow {
    name?: string;
    language?: string;
    translations?: string;
    confirmed?: boolean;

    constructor(name: string, language: string, translations: string, confirmed: boolean)
    {
        this.name = name;
        this.language = language;
        this.translations = translations;
        this.confirmed = confirmed;
    }
}
