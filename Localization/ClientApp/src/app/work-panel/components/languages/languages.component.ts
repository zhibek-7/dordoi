import { Component, OnInit } from '@angular/core';

import { SharePhraseService } from '../../localServices/share-phrase.service';
import { ShareTranslatedPhraseService } from '../../localServices/share-translated-phrase.service';
import { TranslationService } from '../../../services/translationService.service';
import { ProjectsService } from '../../../services/projects.service';

import { Translation } from '../../../models/database-entities/translation.type';
import { TranslationWithFile } from '../../localEntites/translations/translationWithFile.type';
import { SimilarTranslation } from '../../localEntites/translations/similarTranslation.type';

// import 'jquery-ui/ui/widgets/tabs.js';
import * as $ from 'jquery';
import { ShareWordFromModalService } from '../../localServices/share-word-from-modal.service';

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

    searchByMemoryText: string = "";

    constructor(private sharePhraseService: SharePhraseService, private shareTranslatedPhraseService: ShareTranslatedPhraseService,
                private translationService: TranslationService, private projectService: ProjectsService,
                private shareWordFromModalService: ShareWordFromModalService) {
        
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

        // Событие, срабатываемое при выборе фразы для перевода
        this.sharePhraseService.onClick.subscribe(pickedPhrase => {
            this.searchByMemoryText = null;                         

            // переключает TabBar на вкладку "Предложения языка" при смене слова для перевода
            let activeTab = $(".languagesOptionsBlock .nav-tabs .active").attr('href');

            if(activeTab != "#nav-offers"){
                $("a[href='"+ activeTab +"']").removeClass("active show").attr("aria-selected", false);
                $(activeTab).removeClass("active show")
            }
            $("a[href='#nav-offers']").addClass("active show").attr("aria-selected", true);
            $("#nav-offers").addClass("active show");                                                  
        });                 
        
        // Событие, срабатываемое при введении варианта перевода
        this.shareTranslatedPhraseService.onSumbit.subscribe(translation => this.listOfTranslations.push(translation));

        // Событие, срабатываемое при поиске слова по памяти переводов из модального окна
        this.shareWordFromModalService.onClickFindInMemory.subscribe(word => {
            this.searchByMemoryText = word;

            this.searchByMemory();

            // переключает TabBar на вкладку "Предложения языка" при смене слова для перевода
            let activeTab = $(".languagesOptionsBlock .nav-tabs .active").attr('href');

            if(activeTab != "#nav-memorySearch"){
                $("a[href='"+ activeTab +"']").removeClass("active show").attr("aria-selected", false);
                $(activeTab).removeClass("active show")
            }
            $("a[href='#nav-memorySearch']").addClass("active show").attr("aria-selected", true);
            $("#nav-memorySearch").addClass("active show");  
        });
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
            this.searchByMemory();
        }
    }

    // Поиск по памяти переводов
    searchByMemory(){
        let currentProjectId = this.projectService.currentProjectId;

        this.translationService.findTranslationByMemory(currentProjectId, this.searchByMemoryText)
            .subscribe(
                translations => {
                    this.listOfTranslationsByMemory = translations;
                }
            )

        this.searchByMemoryText = null;
    }

}
