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
    
}