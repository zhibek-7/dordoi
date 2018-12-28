import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { SharePhraseService } from '../../localServices/share-phrase.service';
import { ShareTranslatedPhraseService } from '../../localServices/share-translated-phrase.service';
import { TranslationService } from '../../../services/translationService.service';
import { TranslationSubstringService } from 'src/app/services/translationSubstring.service';

import { TranslationSubstring } from '../../../models/database-entities/translationSubstring.type';
import { Translation } from '../../../models/database-entities/translation.type';

declare var $: any;

@Component({
    selector: 'translation-component',
    templateUrl: './translation.component.html',
    styleUrls: ['./translation.component.css'],
    providers: [TranslationSubstringService]
})
export class TranslationComponent implements OnInit {

    showContextBlock: boolean = true;
    showLeftBlock: boolean = true;
    showRightBlock: boolean = true;
    @Output() onChangedLeftBlock = new EventEmitter<boolean>();
    @Output() onChangedRightBlock = new EventEmitter<boolean>();

    translatedText: string;
    phraseForTranslate: TranslationSubstring;
    translatedPhrase: Translation;


    constructor(private sharePhraseService: SharePhraseService,
        private shareTranslatedPhraseService: ShareTranslatedPhraseService, 
        private translationService: TranslationService,
        private translationSubstringService: TranslationSubstringService ) {

        // Событие, срабатываемое при выборе фразы для перевода
        this.sharePhraseService.onClick.subscribe(pickedPhrase => {
                this.phraseForTranslate = pickedPhrase;                
                this.translatedText = null;
            });
     }

    ngOnInit(): void { }

    // Скрывает левый блок (Блок с фразами)
    hideLeftBlock(){
        if(this.showLeftBlock){
            this.showLeftBlock = false;
        }
        else {
            this.showLeftBlock = true;
        }
        this.onChangedLeftBlock.emit(this.showLeftBlock);        
    }

    // Скрывает правый блок (Блок с комментариями)
    hideRightBlock(){
        if(this.showRightBlock){
            this.showRightBlock = false;
        }
        else {
            this.showRightBlock = true;
        }
        this.onChangedRightBlock.emit(this.showRightBlock);          
    }   

    // Показать / Скрыть контекст
    contextShowClick(){
        if(this.showContextBlock){
            this.showContextBlock = false;
        }
        else {
            this.showContextBlock = true;
        }
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

    // Событие, срабатывающее при сохранении изменений в модальном окне
    enterContext(changedTranslationSubstring: TranslationSubstring){
        this.phraseForTranslate.context = changedTranslationSubstring.context;
        this.phraseForTranslate.translationMaxLength = changedTranslationSubstring.translationMaxLength;


        this.translationSubstringService.updateTranslationSubstring(this.phraseForTranslate)
                                            .subscribe();
    }
    
    // Сохранить вариант перевода
    async submitTranslate(){
        this.translatedPhrase = new Translation(this.translatedText, this.phraseForTranslate.id, 10123, 300);  //поменять потом на реальный id пользователя и id языка
        let insertedTranslationId = await this.translationService.createTranslate(this.translatedPhrase);        
        this.translatedPhrase.id = insertedTranslationId;

        await this.translationService.getAllTranslationsInStringById(this.phraseForTranslate.id);
        this.shareTranslatedPhraseService.sumbitTranslatedPhrase(this.translatedPhrase);

        this.translatedText = null;
        this.translatedPhrase = null;       
        
        $("#btnSave").attr("disabled", true); 
        $("#btnSave").attr("disabled", false);  // хорошо бы найти стиль который убирает обводку кнопки после нажатия(убирать его другим способом)
    }

    // Проверка наличия выбранной фразы
    checkPhrase(): boolean{
        if(this.sharePhraseService.getSharedPhrase() === undefined) {
            return false;
        }
        else {
            return true;
        }
    }    

}
