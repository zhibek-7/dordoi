export class Translation {
    public id?: number;
    public iD_String?: number;
    public translated?: string;
    public confirmed?: boolean;
    public iD_User?: number;
    public user_Name?: string;
    public dateTime?: Date;
    public iD_Locale?: number;
    public selected?: boolean;

    constructor(translated: string, id_string: number, id_locale: number,  id?: number){
        this.id = id;
        this.iD_String = id_string;
        this.translated = translated;
        this.confirmed = false;
        this.dateTime = new Date(Date.now());
        this.iD_Locale = id_locale;
    }
}