import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Translation } from 'src/app/models/database-entities/translation.type';

@Injectable()
export class TranslationService {

    private url = "http://localhost:50872/api/translation/";

    constructor(private http: HttpClient) {

    }

    async createTranslate(translate: Translation){
        let asyncResult = await this.http.post(this.url, translate).toPromise();
        return asyncResult;
    }

    async getAllTranslationsInStringById(idString: number){
        let translations: Translation[] = await this.http.get<Translation[]>(this.url + '/InString/' + idString).toPromise();
        return translations;
    }

}