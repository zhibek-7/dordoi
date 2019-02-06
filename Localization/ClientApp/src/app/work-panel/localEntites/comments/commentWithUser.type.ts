import { Image } from "src/app/models/database-entities/image.type";

export class CommentWithUser {
    public constructor(
        public commentId?: number,
        public comment: string = "",
        public dateTime?: Date,
        public user_Id?: number,
        public user_Name?: string,

        public images: Image[] = undefined
    ) { }
  }
