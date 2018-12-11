import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { String } from '../models/database-entities/string.type';

@Injectable()
export class StringService {

    private url: string = document.getElementsByTagName('base')[0].href + "api/string/";

    constructor(private http: HttpClient) {

    }

    async getStringById(){        
        let asyncResult = await this.http.get<String>(this.url).toPromise();
        return asyncResult;    
    }

    async getStrings(){
        let strings: String[] = await this.http.get<String[]>(this.url).toPromise();
        console.log(strings);
        return strings;
    }
}
