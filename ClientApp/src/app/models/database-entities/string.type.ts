export class String {

    id?: number;
    substringToTranslate?: string;
    description?: string;
    context?: string;
    id_fileOwner?: number;
    positionInFile?: number;
    originalString?: string;
    hasTranslationSubstring?: boolean;
    translationSubstring?: string;
    translationMaxLength?: number;
    translationSubstringPositionInLine?: number;

    constructor(
        id?: number,
        substringToTranslate?: string,
        description?: string,
        context?: string,
        id_fileOwner?: number,
        positionInFile?: number,
        originalString?: string,
        hasTranslationSubstring?: boolean,
        translationSubstring?: string,
        translationMaxLength?: number,
        translationSubstringPositionInLine?: number
    )
    { 
        this.id = id;
        this.substringToTranslate = substringToTranslate;
        this.description = description;
        this.context = context;
        this.id_fileOwner = id_fileOwner;
        this.positionInFile = positionInFile;
        this.originalString = originalString;
        this.hasTranslationSubstring = hasTranslationSubstring;
        this.translationSubstring = translationSubstring;
        this.translationMaxLength = translationMaxLength;
        this.translationSubstringPositionInLine = translationSubstringPositionInLine;
    }
    
}