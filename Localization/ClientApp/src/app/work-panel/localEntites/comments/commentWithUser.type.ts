import { Image } from "src/app/models/database-entities/image.type";

export class CommentWithUser {
  public constructor(
    public comment_Id?: number,
    public comment_text: string = "",
    public dateTime?: Date,
    public user_Id?: number,
    public user_Name?: string,
    public images: Image[] = undefined
  ) {}
}
