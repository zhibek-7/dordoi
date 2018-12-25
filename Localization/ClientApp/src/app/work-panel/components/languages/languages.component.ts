import { Component, OnInit } from '@angular/core';

import { SharePhraseService } from '../../localServices/share-phrase.service';
import { ShareTranslatedPhraseService } from '../../localServices/share-translated-phrase.service';
import { TranslationService } from '../../../services/translationService.service';
import { ProjectsService } from '../../../services/projects.service';

import { Translation } from '../../../models/database-entities/translation.type';
import { TranslationWithFile } from '../../localEntites/translations/translationWithFile.type';

declare var $: any;

@Component({
    selector: 'languages-component',
    templateUrl: './languages.component.html',
    styleUrls: ['./languages.component.css']
})
export class LanguagesComponent implements OnInit {

    listOfTranslations: Translation[];
    listOfTranslationsByMemory: TranslationWithFile[];
    newTranslation: Translation;

    searchByMemory: string = "";

    constructor(private sharePhraseService: SharePhraseService, private shareTranslatedPhraseService: ShareTranslatedPhraseService,
                private translationService: TranslationService, private projectService: ProjectsService) {
        
        this.listOfTranslations = [];
        this.listOfTranslationsByMemory = [];

        this.sharePhraseService.onClick2.subscribe(translationsOfTheString => {
            this.listOfTranslations = translationsOfTheString;
            this.listOfTranslationsByMemory = [];  
        });
        this.shareTranslatedPhraseService.onSumbit.subscribe(translation => this.listOfTranslations.push(translation));
    }


    ngOnInit(): void { }    
    

    checkPhrase(): boolean{
        if(this.sharePhraseService.getSharedPhrase() === undefined) {
            return false;
        }
        else {
            return true;
        }
    }

    checkNumberOfTranslations(){
        if(this.listOfTranslations.length == 0){
            return false;
        }
        else {
            return true;
        }
    }

    checkNumberOfTranslationsByMemory(){
        if(this.listOfTranslationsByMemory.length == 0){
            return false;
        }
        else {
            return true;
        }
    }

    async changeTranslate(translation: Translation){
        if(translation.confirmed == false){
            this.translationService.rejectTranslate(translation.id);
        }
        else {
            this.translationService.acceptTranslate(translation.id);
        }
    }

    async deleteTranslateClick(translationId: number){        
        await this.translationService.deleteTranslate(translationId);
        for(var i = 0; i < this.listOfTranslations.length; i++) {
            if(this.listOfTranslations[i].id == translationId) {
                this.listOfTranslations.splice(i, 1);
                break;
            }
        }
    }

    onEnterPress(event: any){
        if(event.which == 13 || event.keyCode == 13){

            let translationText = event.target.value; 
            let currentProjectId = this.projectService.currentProjectId;
            this.translationService.findTranslationByMemory(currentProjectId, translationText)
                .subscribe(
                    translations => {
                        this.listOfTranslationsByMemory = translations;
                    }
                )
            event.target.value = null;
        }
    }

}
