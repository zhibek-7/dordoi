import { Injectable, EventEmitter } from '@angular/core';
import { String } from 'src/app/models/database-entities/string.type';

@Injectable()
export class SharePhraseService {

    private sharedPhrase: String = undefined;

    onClick:EventEmitter<String> = new EventEmitter();

    constructor(){

    }

    addSharedPhrase(sharedPhrase){
        this.sharedPhrase = sharedPhrase;
        this.onClick.emit(sharedPhrase);
    }

    getSharedPhrase(): String{
        return this.sharedPhrase;
    }

}