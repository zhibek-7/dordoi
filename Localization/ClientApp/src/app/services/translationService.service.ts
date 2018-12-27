import { Injectable } from '@angular/core';
import { HttpClient, HttpParams} from '@angular/common/http';

import { Translation } from '../models/database-entities/translation.type';
import { TranslationWithFile } from '../work-panel/localEntites/translations/translationWithFile.type';
import { SimilarTranslation } from '../work-panel/localEntites/translations/similarTranslation.type';

import { Observable } from 'rxjs';

@Injectable()
export class TranslationService {

    private url: string = document.getElementsByTagName('base')[0].href + "api/translation";

    constructor(private http: HttpClient) {
        
    }

    async createTranslate(translate: Translation){
        return await this.http.post<number>(this.url, translate).toPromise();
    }

    async getAllTranslationsInStringById(idString: number){
        let translations: Translation[] = await this.http.get<Translation[]>(this.url + '/InString/' + idString).toPromise();
        return translations;
    }

    async deleteTranslate(idTranslation: number){
        await this.http.delete(this.url + '/DeleteTranslation/' + idTranslation).toPromise();
    }

    async acceptTranslate(translationId: number){
        await this.http.put(this.url + '/AcceptTranslation/' + translationId, true).toPromise();
    }

    async rejectTranslate(translationId: number){
        await this.http.put(this.url + '/RejectTranslation/' + translationId, false).toPromise();
    }

    updateTranslation(updatedTranslation: Translation): Observable<Object> {
        return this.http.put(this.url + updatedTranslation.id, updatedTranslation);
    }

    findTranslationByMemory(currentProjectId: number, translationText: string): Observable<TranslationWithFile[]>{
        const params = new HttpParams().set('currentProjectId', currentProjectId.toString())
                                        .append('translationText', translationText);
        return this.http.get<TranslationWithFile[]>(this.url + '/FindTranslationByMemory/', {params});
    }

    findSimilarTranslations(currentProjectId: number, translationText: string): Observable<SimilarTranslation[]>{
        const params = new HttpParams().set('currentProjectId', currentProjectId.toString())
                                        .append('translationText', translationText);
        return this.http.get<SimilarTranslation[]>(this.url + '/FindSimilarTranslations/', {params});
    }

}
