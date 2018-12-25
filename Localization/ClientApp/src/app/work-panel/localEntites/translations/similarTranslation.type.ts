export class SimilarTranslation {
    public constructor(
        public fileOwnerName: string,
        public translationText: string,
        public translationVariant: string,
        public similarity: number
    ) { }
}