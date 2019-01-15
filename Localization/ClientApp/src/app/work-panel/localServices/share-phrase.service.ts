import { Injectable, EventEmitter } from '@angular/core';

import { TranslationService } from '../../services/translationService.service';

import { TranslationSubstring } from '../../models/database-entities/translationSubstring.type';
import { Translation } from '../../models/database-entities/translation.type';

declare var $: any;

@Injectable()
export class SharePhraseService {

  private sharedPhrase: TranslationSubstring = undefined;
  private translationsOfTheString: Translation[];

  onClick: EventEmitter<TranslationSubstring> = new EventEmitter();
  onClick2: EventEmitter<Translation[]> = new EventEmitter();

    constructor(private translateService: TranslationService){

    }

    async addSharedPhrase(sharedPhrase){
        this.sharedPhrase = sharedPhrase;
        this.onClick.emit(sharedPhrase);
    }

    getSharedPhrase(): TranslationSubstring{
        return this.sharedPhrase;
    }

    async getTranslationsOfPickedPhrase(){
        // this.translationsOfTheString = [];
        this.translationsOfTheString = await this.translateService.getAllTranslationsInStringById(this.sharedPhrase.id);
        this.onClick2.emit(this.translationsOfTheString);
    }

}
