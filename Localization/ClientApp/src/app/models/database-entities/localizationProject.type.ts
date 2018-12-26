export class LocalizationProject {
    id: number;
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
    notifyNew?: boolean;         
    notifyFinish?: boolean;   
    notifyConfirm?: boolean;
    logo?: string; // потом поменять
}
