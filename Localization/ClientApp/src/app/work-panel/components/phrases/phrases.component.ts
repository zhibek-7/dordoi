import { Component, OnInit} from '@angular/core';
import { MatIconModule } from '@angular/material'

import { String } from '../../../models/database-entities/string.type';
import { SharePhraseService } from '../../localServices/share-phrase.service';
import { StringService } from '../../../services/stringService.service';

@Component({
    selector: 'phrases-component',
    templateUrl: './phrases.component.html',
    styleUrls: ['./phrases.component.css'],
    providers: [StringService]
})
export class PhrasesComponent implements OnInit {

    phrasesList: Array<String>;
    phrasesOnPages: String[][];
    currentPageOfPhrases: String[];
    filtredPhrases: String[];

    pickedPhrase: String;

    currentPageNumber: number = 1;
    maxNumberOfPages: number;

    constructor(private sharePhraseService: SharePhraseService, private stringService: StringService) {
        this.phrasesList = new Array<String>();
     }

    ngOnInit(): void {
        this.getStrings();
     }

    async getStrings() {
        this.phrasesList = await this.stringService.getStrings();
        this.countPages();
    }

     async countPages() {
        let maxPhrasesOnOnePage = 70;
        this.phrasesOnPages = [];

        let StringsFromNullValue: String[] = [];
        this.phrasesList.forEach(element => {
            if(element.substringToTranslate != null) {
                StringsFromNullValue.push(element);
            }
        });
        let allPhrases = StringsFromNullValue.length;
        let numberOfPages = Math.ceil(allPhrases/maxPhrasesOnOnePage);
        this.phrasesList = StringsFromNullValue;

        for(var i: number = 0; i < numberOfPages; i++){
            this.phrasesOnPages[i] = [];
            for(var j: number = 0; j < maxPhrasesOnOnePage; j++){
                let position: number = ( (i)*maxPhrasesOnOnePage + j );
                if(this.phrasesList[position] != undefined){
                    this.phrasesOnPages[i][j] = StringsFromNullValue[position];
                }
            }
        }
        this.currentPageOfPhrases = [];
        this.currentPageOfPhrases = this.phrasesOnPages[0];
        this.maxNumberOfPages = await this.phrasesOnPages.length;
     }

     previuosPage() {
         if(this.currentPageNumber > 1){
            --this.currentPageNumber;
            this.currentPageOfPhrases = this.phrasesOnPages[this.currentPageNumber - 1];
         }
     }

     nextPage() {
        if(this.currentPageNumber < this.maxNumberOfPages){
            ++this.currentPageNumber;
            this.currentPageOfPhrases = this.phrasesOnPages[this.currentPageNumber - 1];
        }
    }

    choosePhrase(phrase){
        this.pickedPhrase = phrase;
        this.sharePhraseService.addSharedPhrase(this.pickedPhrase);
        this.sharePhraseService.getTranslationsOfPickedPhrase();
    }

    filterPhrasesClick(){
        alert("Здесь будут функции фильтрации");
    }
}
