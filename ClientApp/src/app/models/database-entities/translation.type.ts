export class Translation {
    id: number;
    translated: string;
    conformid: boolean;
    dateTime: Date;

    constructor(id: number, translated: string ){
        this.id = id;
        this.translated = translated;
        this.conformid = false;
        this.dateTime = new Date(Date.now());
    }
}