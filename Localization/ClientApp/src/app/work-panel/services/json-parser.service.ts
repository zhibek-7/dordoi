import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Phrase } from '../../entities/phrase/phrase.type';
import { Comment } from '../../entities/comments/comment.type';

@Injectable()
export class JsonParserService {

    phrasesFromFile: Array<Phrase>;

    constructor( private http: HttpClient){

    }

    public async getPhrasesFromJsonFile(filePath:string){
        let asyncResult = await this.http.get<Array<Phrase>>(filePath).toPromise();
        return asyncResult;
    }

    public async getCommentsFromJsonFile(filePath:string){
        let asyncResult = await this.http.get<Array<Comment>>(filePath).toPromise();
        return asyncResult;
    }

    //УСТАРЕВШИЙ МЕТОД ИСПОЛЬЗОВАНИЯ АСИНХРОННЫХ МЕТОДОВ

    // public async getDataFromJsonFile(filePath:string){
    //     let tempData = await this.readFromJsonFile(filePath);
    //     return(tempData);
    // }

    // private readFromJsonFile(filePath:string) {
    //     return new Promise(resolve => {
    //             this.http.get(filePath).subscribe(
    //                 data => {
    //                     this.phrasesFromFile = data as Array<Phrase>;
    //                     resolve(this.phrasesFromFile);
    //                 }                    
    //             );
    //       });
    // }
    

}