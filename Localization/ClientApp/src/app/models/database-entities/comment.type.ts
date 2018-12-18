export class Comment {
    public constructor(        
        public id_User: number,
        public id_TranslationSubstrings: number,
        public comment: string,
        public dateTime: Date = new Date(Date.now()),
        public id?: number
      ) { }   
}