import { Injectable, EventEmitter } from '@angular/core';

@Injectable()
export class ShareWordFromModalService {

    sharedWord: string = undefined;

    onClickFindInMemory: EventEmitter<String> = new EventEmitter();
    onClickFindInGlossary: EventEmitter<String> = new EventEmitter();

    clickFindInMemory(sharedWord: string){
        this.sharedWord = sharedWord.replace(/\s+/g, '');
        this.onClickFindInMemory.emit(this.sharedWord);
    }

    clickFindInGlossary(sharedWord: string){
        this.sharedWord = sharedWord.replace(/\s+/g, '');
        this.onClickFindInGlossary.emit(this.sharedWord);        
    }    

}