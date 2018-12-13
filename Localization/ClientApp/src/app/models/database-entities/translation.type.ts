export class Translation {
    public id?: number;
    public iD_String?: number;
    public translated?: string;
    public confirmed?: boolean;
    public iD_User?: number;
    public dateTime?: Date;
    public iD_Locale?: number;

    constructor(translated: string, id_string: number, id_user: number, id_locale: number,  id?: number){
        this.id = id;
        this.iD_String = id_string;
        this.translated = translated;
        this.confirmed = false;
        this.iD_User = id_user;
        this.dateTime = new Date(Date.now());
        this.iD_Locale = id_locale;
    }
}