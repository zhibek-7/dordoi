export class SimilarTranslation {
  public constructor(
    public file_owner_name: string,
    public translation_text: string,
    public translation_variant: string,
    public similarity: number
  ) {}
}
