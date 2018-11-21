import { Injectable, EventEmitter } from '@angular/core';

@Injectable()
export class ShareTranslatedPhraseService {

    private translatedText: string;

    onSumbit:EventEmitter<string> = new EventEmitter();

    sumbitTranslatedPhrase(translatedText: string){
        this.translatedText = translatedText;
        this.onSumbit.emit(this.translatedText);
    }

    deleteTranlationFromOffers(){
        
    }

}