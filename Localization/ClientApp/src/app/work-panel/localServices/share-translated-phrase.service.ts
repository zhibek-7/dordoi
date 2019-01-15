import { Injectable, EventEmitter } from '@angular/core';

import { Translation } from 'src/app/models/database-entities/translation.type';

@Injectable()
export class ShareTranslatedPhraseService {

    onSumbit:EventEmitter<Translation> = new EventEmitter();

    sumbitTranslatedPhrase(translation: Translation){
        this.onSumbit.emit(translation);        
    }   

}