import { Term } from "src/app/models/Glossaries/term.type";
import { Glossary } from "src/app/models/database-entities/glossary.type";

export class TermWithGlossary {

    get id(): number {
        return this.iD;
    }    

    public constructor(
        public iD?: number,
        public substringToTranslate?: string,
        public description?: string,
        public context?: string,
        public iD_FileOwner?: number,
        public translationMaxLength?: number,
        public value?: string,
        public positionInText?: number,
        public partOfSpeechId?: number,

        public glossaryId?: number,
        public glossaryName?: string,
        public glossaryDescription?: string,

        public term?: Term,
        public glossary?: Glossary
    ) { }
}