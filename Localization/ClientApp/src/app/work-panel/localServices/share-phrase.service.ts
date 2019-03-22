import { Injectable, EventEmitter } from '@angular/core';

import { TranslationService } from '../../services/translationService.service';
import { Guid } from 'guid-typescript';
import { TranslationSubstring } from '../../models/database-entities/translationSubstring.type';
import { Translation } from '../../models/database-entities/translation.type';

//declare var $: any;

@Injectable()
export class SharePhraseService {

  private sharedPhrase: TranslationSubstring = undefined;
  private translationsOfTheString: Translation[];

  onClick: EventEmitter<TranslationSubstring> = new EventEmitter();
  onClick2: EventEmitter<Translation[]> = new EventEmitter();
  onClickCheckBoxChanged: EventEmitter<string> = new EventEmitter();

    constructor(private translateService: TranslationService){

    }

    setStatusOfTranslationSubstring(status: string){
        this.onClickCheckBoxChanged.emit(status);
    }

    async addSharedPhrase(sharedPhrase){
        this.sharedPhrase = sharedPhrase;
        this.onClick.emit(sharedPhrase);
    }

    getSharedPhrase(): TranslationSubstring{
        return this.sharedPhrase;
    }

    setSharedPhraseToNull(){
        this.sharedPhrase = undefined;
    }

    async getTranslationsOfPickedPhrase(localeId: Guid){
        this.translationsOfTheString = await this.translateService.getAllTranslationsInStringByIdAndLocale(this.sharedPhrase.id, localeId);
        this.onClick2.emit(this.translationsOfTheString);
    }

}
