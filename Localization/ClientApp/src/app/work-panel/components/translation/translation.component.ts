import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SharePhraseService } from '../../localServices/share-phrase.service';
import { ShareTranslatedPhraseService } from '../../localServices/share-translated-phrase.service';

import { String } from '../../../models/database-entities/string.type';
import { Translation } from '../../../models/database-entities/translation.type';
import { TranslationService } from '../../../services/translationService.service';

declare var $: any;

@Component({
    selector: 'translation-component',
    templateUrl: './translation.component.html',
    styleUrls: ['./translation.component.css']
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

    async submitTranslate(){
        this.translatedPhrase = new Translation(this.translatedText, this.phraseForTranslate.id, 10123);  //поменять потом на реальный id пользователя
        let createdTranslate: Translation = await this.translationService.createTranslate(this.translatedPhrase);
        await this.translationService.getAllTranslationsInStringById(this.phraseForTranslate.id);
        this.shareTranslatedPhraseService.sumbitTranslatedPhrase(createdTranslate);
        this.translatedText = null;
        this.translatedPhrase = null;       
        $("#btnSave").attr("disabled", true); 
        $("#btnSave").attr("disabled", false);  // хорошо бы найти стиль который убирает обводку кнопки после нажатия(убирать его другим способом)
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
