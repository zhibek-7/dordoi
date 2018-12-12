// Модель проектов
export class Project {
    id: string;
    name: string;
    description: string;
    url: string;
    visibility: boolean;
    dateOfCreation: Date;
    lastActivity: Date;
    ID_SourceLocale: number;
    ableToDownload: boolean;
    ableToLeftErrors: boolean;
    defaultString: string;
    notifyNew: boolean;
    notifyFinish: boolean;
    notifyConfirm: boolean;
    logo: any;

  //Поля ниже не соответствуют БД. Оставил пока что бы ни у кого ничего не сломалось.
    owner: string;
    created: any;
    changed: any;
    hasErr: boolean;
    type: string;
    sourceLanguage: string;
}
