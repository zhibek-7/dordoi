import { Component, OnInit } from '@angular/core';

import { SharePhraseService } from '../../localServices/share-phrase.service';
import { ShareTranslatedPhraseService } from '../../localServices/share-translated-phrase.service';
import { TranslationService } from '../../../services/translationService.service';

import { Translation } from '../../../models/database-entities/translation.type';

@Component({
    selector: 'languages-component',
    templateUrl: './languages.component.html',
    styleUrls: ['./languages.component.css']
})
export class LanguagesComponent implements OnInit {

    listOfTranslations: Translation[];
    newTranslation: Translation;

    constructor(private sharePhraseService: SharePhraseService, private shareTranslatedPhraseService: ShareTranslatedPhraseService,
                private translationService: TranslationService) {
        
        this.listOfTranslations = [];

        this.sharePhraseService.onClick2.subscribe(translationsOfTheString => this.listOfTranslations = translationsOfTheString);
        this.shareTranslatedPhraseService.onSumbit.subscribe(translation => this.listOfTranslations.push(translation));
    }


    ngOnInit(): void { }

    acceptTranslateClick(){
        alert("Перевод принят");
    }

    async rejectTranslateClick(translationId: number){        
        await this.translationService.rejectTranslate(translationId);
        for(var i = 0; i < this.listOfTranslations.length; i++) {
            if(this.listOfTranslations[i].id == translationId) {
                this.listOfTranslations.splice(i, 1);
                break;
            }
        }
    }

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

}
