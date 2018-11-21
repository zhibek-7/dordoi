import { Component, OnInit } from '@angular/core';
import { SharePhraseService } from '../../services/share-phrase.service';
import { ShareTranslatedPhraseService } from '../../services/share-translated-phrase.service';

@Component({
    selector: 'languages-component',
    templateUrl: './languages.component.html',
    styleUrls: ['./languages.component.css']
})
export class LanguagesComponent implements OnInit {

    listOfTranslations: string[];

    constructor(private sharePhraseService: SharePhraseService,private shareTranslatedPhraseService: ShareTranslatedPhraseService) {
        
        this.listOfTranslations = [];

        this.shareTranslatedPhraseService.onSumbit.subscribe(translatedText => this.listOfTranslations.push(translatedText));
    }


    ngOnInit(): void { }

    acceptTranslateClick(){
        alert("Перевод принят");
    }

    rejectTranslateClick(){
        alert("Перевод отклонён");
    }

    checkPhrase(): boolean{
        if(this.sharePhraseService.getSharedPhrase() === undefined) {
            return false;
        }
        else {
            return true;
        }
    }

    checkTranslations(){
        if(this.listOfTranslations.length == 0){
            return false;
        }
        else {
            return true;
        }
    }

}
