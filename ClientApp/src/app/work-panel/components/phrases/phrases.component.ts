import { Component, OnInit} from '@angular/core';

import { Phrase } from '../../../entities/phrase/phrase.type';
import { String } from 'src/app/models/database-entities/string.type';
import { JsonParserService } from '../../services/json-parser.service';
import { SharePhraseService } from '../../services/share-phrase.service';
import { StringService } from '../../services/databaseServices/stringService.service';

@Component({
    selector: 'phrases-component',
    templateUrl: './phrases.component.html',
    styleUrls: ['./phrases.component.css'],
    providers: [JsonParserService, StringService]
})
export class PhrasesComponent implements OnInit {

    phrasesList: Array<String>;
    phrasesOnPages: String[][];
    currentPageOfPhrases: String[];
    filtredPhrases: String[];

    pickedPhrase: String;

    currentPageNumber: number = 1;
    maxNumberOfPages: number;

    constructor(private jsonParserService: JsonParserService,
         private sharePhraseService: SharePhraseService, private stringService: StringService) {
        this.phrasesList = new Array<Phrase>();
     }

    ngOnInit(): void {
        this.getStrings();
     }

    async getStrings(){
        this.phrasesList = await this.stringService.getStrings();
        this.countPages();
    }

     async countPages(){
        let maxPhrasesOnOnePage = 70;
        let allPhrases = this.phrasesList.length;
        let numberOfPages = Math.ceil(allPhrases/maxPhrasesOnOnePage);
        this.phrasesOnPages = [];

        for(var i: number = 0; i < numberOfPages; i++){
            this.phrasesOnPages[i] = [];
            for(var j: number = 0; j < maxPhrasesOnOnePage; j++){
                let position: number = ( (i)*70 + j );
                if(this.phrasesList[position] != undefined && this.phrasesList[position].substringToTranslate != null){
                    this.phrasesOnPages[i][j] = this.phrasesList[position];
                }
            }
        }
        this.currentPageOfPhrases = [];
        this.currentPageOfPhrases = this.phrasesOnPages[0];
        this.maxNumberOfPages = await this.phrasesOnPages.length;
        await console.log(this.phrasesOnPages);
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
    }

    filterPhrasesClick(){
        alert("Здесь будут функции фильтрации");
    }
}
