import { Injectable, EventEmitter } from '@angular/core';

import { Translation } from 'src/app/models/database-entities/translation.type';
import { String } from 'src/app/models/database-entities/string.type';

@Injectable()
export class ShareTranslatedPhraseService {

    onSumbit:EventEmitter<Translation> = new EventEmitter();

    sumbitTranslatedPhrase(translation: Translation){
        this.onSumbit.emit(translation);        
    }

    deleteTranlationFromOffers(){
        
    }

}