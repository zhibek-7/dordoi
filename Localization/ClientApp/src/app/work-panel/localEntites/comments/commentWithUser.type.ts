import { Image } from "src/app/models/database-entities/image.type";
import { Guid } from 'guid-typescript';
export class CommentWithUser {
  public constructor(
    public comment_id?: Guid,
    public comment_text: string = "",
    public datetime?: Date,
    public user_id?: Guid,
    public user_name?: string,
    public images: Image[] = undefined
  ) {}
}
