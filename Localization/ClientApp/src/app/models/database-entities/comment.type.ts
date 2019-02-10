export class Comment {
  public constructor(
    public id_User: number,
    public id_Translation_Substrings: number,
    public comment_text: string,
    public dateTime: Date = new Date(),
    public id?: number
  ) { }
}
