import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { SharePhraseService } from '../../localServices/share-phrase.service';
import { ShareTranslatedPhraseService } from '../../localServices/share-translated-phrase.service';

import { TranslationSubstring } from '../../../models/database-entities/translationSubstring.type';
import { Translation } from '../../../models/database-entities/translation.type';
import { TranslationService } from '../../../services/translationService.service';
import { NullTemplateVisitor } from '@angular/compiler';

declare var $: any;

@Component({
    selector: 'translation-component',
    templateUrl: './translation.component.html',
    styleUrls: ['./translation.component.css']
})
export class TranslationComponent implements OnInit {

    showContextBlock: boolean = true;
    showLeftBlock: boolean = true;
    showRightBlock: boolean = true;
    @Output() onChangedLeftBlock = new EventEmitter<boolean>();
    @Output() onChangedRightBlock = new EventEmitter<boolean>();

    @Input() currentContext: string = null;
    translatedText: string;
    phraseForTranslate: TranslationSubstring;
    translatedPhrase: Translation;


    constructor(private sharePhraseService: SharePhraseService,
        private shareTranslatedPhraseService: ShareTranslatedPhraseService, private translationService: TranslationService ) {

        this.sharePhraseService.onClick.subscribe(pickedPhrase => {
                this.phraseForTranslate = pickedPhrase;                
                this.translatedText = null;
            });
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

    enterContext(context: any){
        this.currentContext = context;
    }
    
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

    checkPhrase(): boolean{
        if(this.sharePhraseService.getSharedPhrase() === undefined) {
            return false;
        }
        else {
            return true;
        }
    }    

}
