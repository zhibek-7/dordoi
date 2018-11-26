export class Translation {
    id: number;
    iD_String: number;
    translated: string;
    conformid: boolean;
    iD_User: number;
    dateTime: Date;

    constructor(translated: string, id_string: number, id_user: number ){
        this.iD_String = id_string;
        this.translated = translated;
        this.conformid = false;
        this.iD_User = id_user;
        this.dateTime = new Date(Date.now());
    }
}