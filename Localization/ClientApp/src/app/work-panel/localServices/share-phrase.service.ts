import { Injectable, EventEmitter } from '@angular/core';

import { TranslationService } from '../../services/accessToDatabase/translationService.service';

import { String } from 'src/app/models/database-entities/string.type';

@Injectable()
export class SharePhraseService {

    private sharedPhrase: String = undefined;
    private translationsOfTheString: String[];

    onClick:EventEmitter<String> = new EventEmitter();
    onClick2:EventEmitter<String[]> = new EventEmitter();

    constructor(private translateService: TranslationService){

    }

    async addSharedPhrase(sharedPhrase){
        this.sharedPhrase = sharedPhrase;
        this.onClick.emit(sharedPhrase);
    }

    getSharedPhrase(): String{
        return this.sharedPhrase;
    }

    async getTranslationsOfPickedPhrase(){
        this.translationsOfTheString = [];
        this.translationsOfTheString = await this.translateService.getAllTranslationsInStringById(this.sharedPhrase.id);
        this.onClick2.emit(this.translationsOfTheString);
    }

}