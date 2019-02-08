import { Image } from "src/app/models/database-entities/image.type";

export class CommentWithUser {
  public constructor(
    public comment_id?: number,
    public comment_text: string = "",
    public datetime?: Date,
    public user_id?: number,
    public user_name?: string,
    public images: Image[] = undefined
  ) {}
}
