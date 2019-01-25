import { Image } from "src/app/models/database-entities/image.type";

export class CommentWithUser {
    public constructor(
        public commentId?: number,
        public comment: string = "",
        public dateTime?: Date,
        public userId?: number,
        public userName?: string,

        public images: Image[] = undefined
    ) { }
  }