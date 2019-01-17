export class TermWithGlossary {
    public constructor(
        public id: number,
        public termText: string,
        public termDesciption: string,
        public glossaryId: number,
        public glossaryName: string
    ) { }
}