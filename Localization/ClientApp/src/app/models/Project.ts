// Модель проектов
export class Project {
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

        //public dateTime: Date = new Date(Date.now()),
        //public id?: number
    public constructor(
      name: string,
      description: string,
      url: string
          ) {
              this.name = name;
              this.description = description;
              this.url = url;
             // this.visibility = false;
              this.dateOfCreation  = "12.12.2018";
              this.lastActivity = "12.12.2018";
              //this.ableToDownload = false;
             // this.ableToLeftErrors = false;
             // this.defaultString = '123';
              //this.notifyNew = false;
              //this.notifyFinish = false;
             // this.logo = []];


           }
}
