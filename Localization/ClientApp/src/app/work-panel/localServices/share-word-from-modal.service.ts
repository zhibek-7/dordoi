import { Injectable, EventEmitter } from '@angular/core';

@Injectable()
export class ShareWordFromModalService {

    sharedWord: string = undefined;

    onClickFindInMemory: EventEmitter<String> = new EventEmitter();

    clickFindInMemory(sharedWord: string){
        this.sharedWord = sharedWord;
        this.onClickFindInMemory.emit(this.sharedWord);
    }

    clickFindInGlossary(){
    }

    

}