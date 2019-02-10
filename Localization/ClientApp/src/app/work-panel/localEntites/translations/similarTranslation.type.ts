export class SimilarTranslation {
  public constructor(
    public file_Owner_Name: string,
    public translation_Text: string,
    public translation_Variant: string,
    public similarity: number
  ) {}
}
