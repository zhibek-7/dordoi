export class Translator {
  constructor(
    public user_Id?: number,
    public publicProfile?: boolean,
    public user_Name?: string,
    public user_pic?: any,
    public translationRating?: number,
    public termRating?: number,
    public topics?: Array<string>,
    public wordsQuantity?: number,
    public cost?: number
  ) { }
}
