import { Component, OnInit, Input } from "@angular/core";
import { MatIconModule } from "@angular/material";

import { TranslationSubstring } from "../../../models/database-entities/translationSubstring.type";
import { SharePhraseService } from "../../localServices/share-phrase.service";
import { TranslationSubstringService } from "../../../services/translationSubstring.service";

@Component({
  selector: "phrases-component",
  templateUrl: "./phrases.component.html",
  styleUrls: ["./phrases.component.css"],
  providers: [TranslationSubstringService]
})
export class PhrasesComponent implements OnInit {
  searchText: string = "";
  phrasesList: TranslationSubstring[];
  phrasesOnPages: TranslationSubstring[][];
  currentPageOfPhrases: TranslationSubstring[];
  filtredPhrases: TranslationSubstring[];

  pickedPhrase: TranslationSubstring;

  currentPageNumber: number = 1;
  maxNumberOfPages: number;

  @Input() fileId: number;

  constructor(
    private sharePhraseService: SharePhraseService,
    private stringService: TranslationSubstringService
  ) {
    this.phrasesList = new Array<TranslationSubstring>();
  }

  ngOnInit(): void {
    this.getStrings();
  }

  // Получение всех фраз для перевода
  async getStrings() {
    this.phrasesList = await this.stringService
      .getStringsInFile(this.fileId)
      .toPromise();
    this.countPages();
  }

  // Расчет кол-ва страниц с фразами для перевода
  async countPages() {
    let maxPhrasesOnOnePage = 70;
    this.phrasesOnPages = [];

    let StringsFromNullValue: TranslationSubstring[] = [];

    this.phrasesList.forEach(element => {
      if (element.substring_to_translate != null) {
        StringsFromNullValue.push(element);
      }
    });

    let allPhrases = StringsFromNullValue.length;
    let numberOfPages = Math.ceil(allPhrases / maxPhrasesOnOnePage);
    this.phrasesList = StringsFromNullValue;

    for (let i: number = 0; i < numberOfPages; i++) {
      this.phrasesOnPages[i] = [];
      for (let j: number = 0; j < maxPhrasesOnOnePage; j++) {
        let position: number = i * maxPhrasesOnOnePage + j;
        if (this.phrasesList[position] != undefined) {
          this.phrasesOnPages[i][j] = StringsFromNullValue[position];
        }
      }
    }
    this.currentPageOfPhrases = [];
    this.currentPageOfPhrases = this.phrasesOnPages[0];
    this.maxNumberOfPages = await this.phrasesOnPages.length;
  }

  // Переход на предыдущую страницу с фразами для перевода
  previuosPage() {
    if (this.currentPageNumber > 1) {
      --this.currentPageNumber;
      this.currentPageOfPhrases = this.phrasesOnPages[
        this.currentPageNumber - 1
      ];
    }
  }

  // Переход на предыдущую страницу с фразами для перевода
  nextPage() {
    if (this.currentPageNumber < this.maxNumberOfPages) {
      ++this.currentPageNumber;
      this.currentPageOfPhrases = this.phrasesOnPages[
        this.currentPageNumber - 1
      ];
    }
  }

  // Событие, срабатываемое при нажатии на фразу для перевода
  choosePhrase(phrase) {
    this.pickedPhrase = phrase;
    this.sharePhraseService.addSharedPhrase(this.pickedPhrase);
    this.sharePhraseService.getTranslationsOfPickedPhrase();
  }

  searchInputEmpty(): boolean {
    if (this.searchText == "" || this.searchText == null) {
      return true;
    } else {
      return false;
    }
  }

  filterPhrasesClick() {
    alert("Здесь будут функции фильтрации");
  }
}
