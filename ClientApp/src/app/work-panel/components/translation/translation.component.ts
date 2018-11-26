import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SharePhraseService } from '../../services/share-phrase.service';
import { ShareTranslatedPhraseService } from '../../services/share-translated-phrase.service';



import { String } from 'src/app/models/database-entities/string.type';
import { Translation } from 'src/app/models/database-entities/translation.type';
import { TranslationService } from '../../services/databaseServices/translationService.service';

// import { Phrase } from '../../../entities/phrase/phrase.type';


@Component({
    selector: 'translation-component',
    templateUrl: './translation.component.html',
    styleUrls: ['./translation.component.css'],
    providers: [TranslationService]
})
export class TranslationComponent implements OnInit {

    showLeftBlock: boolean = true;
    showRightBlock: boolean = true;
    @Output() onChangedLeftBlock = new EventEmitter<boolean>();
    @Output() onChangedRightBlock = new EventEmitter<boolean>();

    translatedText: string;
    phraseForTranslate: String;
    translatedPhrase: Translation;


    constructor(private sharePhraseService: SharePhraseService,
        private shareTranslatedPhraseService: ShareTranslatedPhraseService, private translationService: TranslationService ) {

        this.sharePhraseService.onClick.subscribe(pickedPhrase => this.phraseForTranslate = pickedPhrase);
     }

    ngOnInit(): void { }

    hideLeftBlock(){
        if(this.showLeftBlock){
            this.showLeftBlock = false;
        }
        else {
            this.showLeftBlock = true;
        }
        this.onChangedLeftBlock.emit(this.showLeftBlock);        
    }

    hideRightBlock(){
        if(this.showRightBlock){
            this.showRightBlock = false;
        }
        else {
            this.showRightBlock = true;
        }
        this.onChangedRightBlock.emit(this.showRightBlock);          
    }

    editContextClick(){
        alert("Редактируем контекст");
    }

    contextShowClick(){
        alert("Показываем контекст");
    }

    arrowLeftClick(){
        alert("Действие при нажатии левой стрелки");
    }

    arrowRightClick(){
        alert("Действие при нажатии правой стрелки");
    }

    importFileClick(){
        alert("Импортируем файл");
    }

    async submitTranslate(){ // завершить данный метод
        this.translatedPhrase = new Translation(this.translatedText, this.phraseForTranslate.id, 10123);  //поменять потом на реальный id пользователя
        let test = await this.translationService.createTranslate(this.translatedPhrase);
        // let test = await this.translationService.getTranslations();
        console.log(test);

        // this.shareTranslatedPhraseService.sumbitTranslatedPhrase(this.translatedPhrase);
        // this.translatedPhrase = null;
    }

    checkPhrase(): boolean{
        if(this.sharePhraseService.getSharedPhrase() === undefined) {
            return false;
        }
        else {
            return true;
        }
    }

}
