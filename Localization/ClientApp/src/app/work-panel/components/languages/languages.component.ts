import { Component, OnInit } from '@angular/core';

import { SharePhraseService } from '../../localServices/share-phrase.service';
import { ShareTranslatedPhraseService } from '../../localServices/share-translated-phrase.service';
import { TranslationService } from '../../../services/translationService.service';
import { ProjectsService } from '../../../services/projects.service';

import { Translation } from '../../../models/database-entities/translation.type';
import { TranslationWithFile } from '../../localEntites/translations/translationWithFile.type';
import { SimilarTranslation } from '../../localEntites/translations/similarTranslation.type';

declare var $: any;

@Component({
    selector: 'languages-component',
    templateUrl: './languages.component.html',
    styleUrls: ['./languages.component.css']
})
export class LanguagesComponent implements OnInit {

    listOfTranslations: Translation[];

    listOfTranslationsByMemory: TranslationWithFile[];

    listOfSimilarTranslations: SimilarTranslation[];

    newTranslation: Translation;

    searchByMemory: string = "";

    constructor(private sharePhraseService: SharePhraseService, private shareTranslatedPhraseService: ShareTranslatedPhraseService,
                private translationService: TranslationService, private projectService: ProjectsService) {
        
        this.listOfTranslations = [];
        this.listOfTranslationsByMemory = [];
        this.listOfSimilarTranslations = [];

        // Событие, срабатываемое при выборе фразы для перевода
        this.sharePhraseService.onClick2.subscribe(translationsOfTheString => {
            this.listOfTranslations = translationsOfTheString;
            this.listOfTranslationsByMemory = [];  
            this.listOfSimilarTranslations = [];
        
            this.findSimilarTranslations();
        });

        // Событие, срабатываемое при введении варианта перевода
        this.shareTranslatedPhraseService.onSumbit.subscribe(translation => this.listOfTranslations.push(translation));
    }


    ngOnInit(): void { }    
    

    // Функция проверки наличия фразы
    checkPhrase(): boolean{
        if(this.sharePhraseService.getSharedPhrase() === undefined) {
            return false;
        }
        else {
            return true;
        }
    }

    // Функция проверки кол-ва вариантов перевода
    checkNumberOfTranslations(){
        if(this.listOfTranslations.length == 0){
            return false;
        }
        else {
            return true;
        }
    }

    // Функция проверки кол-ва вариантов перевода по поиску по памяти
    checkNumberOfTranslationsByMemory(){
        if(this.listOfTranslationsByMemory.length == 0){
            return false;
        }
        else {
            return true;
        }
    }

    // Функция проверки кол-ва схожих вариантов перевода
    checkNumberOfSimilarTranslations(){
        if(this.listOfSimilarTranslations.length == 0){
            return false;
        }
        else {
            return true;
        }
    }

    // Изменение подтвержедние перевода
    async changeTranslate(translation: Translation){
        if(translation.confirmed == false){
            this.translationService.rejectTranslate(translation.id);
        }
        else {
            this.translationService.acceptTranslate(translation.id);
        }
    }

    // Удаление варианта перевода
    async deleteTranslateClick(translationId: number){        
        await this.translationService.deleteTranslate(translationId);
        for(var i = 0; i < this.listOfTranslations.length; i++) {
            if(this.listOfTranslations[i].id == translationId) {
                this.listOfTranslations.splice(i, 1);
                break;
            }
        }
    }

    // Поиск схожих вариантов перевода по нечеткому поиску
    findSimilarTranslations(){
        let phraseForTranslation = this.sharePhraseService.getSharedPhrase();
        let currentProjectId = this.projectService.currentProjectId;

        this.translationService.findSimilarTranslations(currentProjectId, phraseForTranslation)
                .subscribe(
                    similarTranslations => {

                        // Функция сортировки списка схожих вариантов перевода по убыванию
                        function sortBySimilarValue(a,b) {
                            if (a.similarity > b.similarity)
                                return -1;
                            if (a.similarity < b.similarity)
                                return 1;
                            return 0;
                        }
                          
                        similarTranslations.sort(sortBySimilarValue);                                              
                        this.listOfSimilarTranslations = similarTranslations;
                    }
                )
    }

    // Событие, срабатываемое при нажатии клавиши Enter в поле "Поиск по памяти"
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
