// Модель проектов
export class Project {
    id: number;
    name: string;
    description: string;
    url: string;
    visibility: boolean;
    dateOfCreation: any;
    lastActivity: any;
    ID_SourceLocale: number;
    ableToDownload: boolean;
    ableToLeftErrors: boolean;
    defaultString: string;
    notifyNew: boolean;
    notifyFinish: boolean;
    notifyConfirm: boolean;
    logo: any;
}
