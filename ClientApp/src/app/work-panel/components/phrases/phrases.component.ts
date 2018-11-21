import { Component, OnInit} from '@angular/core';

import { Phrase } from '../../../entities/phrase/phrase.type';
import { JsonParserService } from '../../services/json-parser.service';
import { SharePhraseService } from '../../services/share-phrase.service';

@Component({
    selector: 'phrases-component',
    templateUrl: './phrases.component.html',
    styleUrls: ['./phrases.component.css'],
    providers: [JsonParserService]
})
export class PhrasesComponent implements OnInit {

    phrasesList: Array<Phrase>;
    phrasesOnPages: Phrase[][];
    currentPageOfPhrases: Phrase[];
    filtredPhrases: Phrase[];

    pickedPhrase: Phrase;

    currentPageNumber: number = 1;
    maxNumberOfPages: number;

    constructor(private jsonParserService: JsonParserService,
         private sharePhraseService: SharePhraseService) {
        this.phrasesList = new Array<Phrase>();
     }

    ngOnInit(): void {
        this.getPhrases();
     }

     async countPages(){
        let maxPhrasesOnOnePage = 15;
        let allPhrases = this.phrasesList.length;
        let numberOfPages = Math.ceil(allPhrases/maxPhrasesOnOnePage);
        this.phrasesOnPages = [];

        for(var i: number = 0; i < numberOfPages; i++){
            this.phrasesOnPages[i] = [];
            for(var j: number = 0; j < maxPhrasesOnOnePage; j++){
                let position: number = ( (i)*15 + j );
                if(this.phrasesList[position] != undefined){
                    this.phrasesOnPages[i][j] = this.phrasesList[position];
                }
            }
        }
        this.currentPageOfPhrases = [];
        this.currentPageOfPhrases = this.phrasesOnPages[0];
        this.maxNumberOfPages = this.phrasesOnPages.length;
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

    async getPhrases() { 
        this.phrasesList = await this.jsonParserService.getPhrasesFromJsonFile('../../../assets/testData/phrases.json');
        await this.countPages();
    }

    choosePhrase(phrase){
        this.pickedPhrase = phrase;
        this.sharePhraseService.addSharedPhrase(this.pickedPhrase);        
    }

    filterPhrasesClick(){
        alert("Здесь будут функции фильтрации");
    }
}
