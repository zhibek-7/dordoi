import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Translation } from '../models/database-entities/translation.type';

@Injectable()
export class TranslationService {

    private url: string = document.getElementsByTagName('base')[0].href + "api/translation/";

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

    async rejectTranslate(idTranslation: number){
        await this.http.delete(this.url + '/RejectTranslation/' + idTranslation).toPromise();
    }

}