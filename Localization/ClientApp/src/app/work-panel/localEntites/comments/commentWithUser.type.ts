export class CommentWithUser {
    public constructor(
        public commentId?: number,
        public comment?: string,
        public dateTime?: Date,
        public userId?: number,
        public userName?: string
    ) { }
  }