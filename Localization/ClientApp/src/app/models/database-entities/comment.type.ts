import { Guid } from "guid-typescript";
export class Comment {
  public id: string;
  public ID_User: string;
  public ID_Translation_Substrings: string;
  public Comment_text: string;
  public DateTime: Date;

  public constructor(ID_Translation_Substrings: string, Comment_text: string) {
    this.ID_User = null; //Guid.createEmpty()
    this.ID_Translation_Substrings = ID_Translation_Substrings + "";
    this.Comment_text = Comment_text;
    this.DateTime = null;
  }
}
