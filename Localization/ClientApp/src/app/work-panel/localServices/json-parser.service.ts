import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class JsonParserService {

    constructor( private http: HttpClient){

    }

    public async getPhrasesFromJsonFile(filePath:string){
        let asyncResult = await this.http.get<Array<String>>(filePath).toPromise();
        return asyncResult;
    }

    public async getCommentsFromJsonFile(filePath:string){
        let asyncResult = await this.http.get<Array<String>>(filePath).toPromise();
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