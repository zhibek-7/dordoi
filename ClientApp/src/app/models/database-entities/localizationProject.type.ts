export class LocalizationProject {
    id: number;
    name: string;
    description: string;
    url: string;
    visibility: boolean;
    dateOfCreateion: Date;
    lastActivity: Date;
    ableToDownload: boolean;
    ableToLeftErrors: boolean;
    defualtString: boolean;
    notifyNew?: boolean;         
    notifyFinish?: boolean;   
    notifyConfirm?: boolean;
    logo?: string; // потом поменять
}