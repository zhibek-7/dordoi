import { Injectable, EventEmitter } from '@angular/core';

import { Translation } from 'src/app/models/database-entities/translation.type';
import { SimilarTranslation } from '../localEntites/translations/similarTranslation.type';
import { TranslationWithLocaleText } from '../localEntites/translations/translationWithLocaleText.type';
import { TranslationWithFile } from '../localEntites/translations/translationWithFile.type';

@Injectable()
export class ShareTranslatedPhraseService {

    onSumbit:EventEmitter<Translation> = new EventEmitter();

    onClickTranslation:EventEmitter<SimilarTranslation 
    | TranslationWithLocaleText 
    | TranslationWithFile 
    | Translation> = new EventEmitter();

    onClickCurrentTranslation: EventEmitter<Translation> = new EventEmitter();

    sumbitTranslatedPhrase(translation: Translation){
        this.onSumbit.emit(translation);        
    }

    submitClickedTranslation(similarTranslation: SimilarTranslation 
        | TranslationWithLocaleText 
        | TranslationWithFile 
        | Translation){

        this.onClickTranslation.emit(similarTranslation);        
    }    

    submitClickOnCurrentTranslation(clickedTranslation: Translation){
        this.onClickCurrentTranslation.emit(clickedTranslation);
    }

}