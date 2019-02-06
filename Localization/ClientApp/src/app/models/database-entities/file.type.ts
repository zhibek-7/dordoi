export class File {
    id: number;
    name: string;
    description: string;
    dateOfChange: Date;
    stringsCount: number;
    version: number;
    priority: number;
    path: string;
    outputName: string;
    isFolder: boolean;
    folder: string;
    id_LocalizationProject: number;
    id_FolderOwner: number;
    isLastVersion: boolean;
    id_PreviousVersion: number;
    translatorName: string;
    downloadName: string;
    percentOfTranslation?: number;
    percentOfConfirmed?: number;
}
