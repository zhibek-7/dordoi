import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { String } from 'src/app/models/database-entities/string.type';


@Injectable()
export class StringService {

    private url = "http://localhost:51931/api/string/";

    constructor(private http: HttpClient) {

    }

    async getStringById(){        
        let asyncResult = await this.http.get<String>(this.url).toPromise();
        await console.log(asyncResult);
        return asyncResult;    
    }

    async getStrings(){
        let strings: String[] = await this.http.get<String[]>(this.url).toPromise();
        await console.log(strings);
        return strings;
    }

    async test(){
        await console.log("Промежуток");
    }
}