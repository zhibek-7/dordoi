import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders  } from '@angular/common/http';

@Injectable()
export class JsonParserService {

    constructor( private http: HttpClient){

    }

    public async getPhrasesFromJsonFile(filePath:string){
        let asyncResult = await this.http.get<Array<String>>(filePath,
      {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))
    }).toPromise();
        return asyncResult;
    }

    public async getCommentsFromJsonFile(filePath:string){
        let asyncResult = await this.http.get<Array<String>>(filePath,
      {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))
    }).toPromise();
        return asyncResult;
    }  
    
}