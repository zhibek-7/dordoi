import { Injectable, EventEmitter } from '@angular/core';
import { Phrase } from '../../entities/phrase/phrase.type';

@Injectable()
export class SharePhraseService {

    private sharedPhrase: Phrase = undefined;

    onClick:EventEmitter<Phrase> = new EventEmitter();

    constructor(){

    }

    addSharedPhrase(sharedPhrase){
        this.sharedPhrase = sharedPhrase;
        this.onClick.emit(sharedPhrase);
    }

    getSharedPhrase(): Phrase{
        return this.sharedPhrase;
    }

}