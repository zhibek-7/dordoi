import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { TranslationSubstring } from '../models/database-entities/translationSubstring.type';
import { Observable } from 'rxjs';

@Injectable()
export class TranslationSubstringService {

    private url: string = "api/string/";

    constructor(private http: HttpClient) {

    }

    async getStringById(){        
      let asyncResult = await this.http.get<TranslationSubstring>(this.url).toPromise();
        return asyncResult;    
    }

    async getStrings(){
      let strings: TranslationSubstring[] = await this.http.get<TranslationSubstring[]>(this.url).toPromise();
        return strings;
    }

  getStringsInFile(idFile: number): Observable<TranslationSubstring[]>{
    return this.http.get<TranslationSubstring[]>(this.url + "InFile/" + idFile);
    }

}
