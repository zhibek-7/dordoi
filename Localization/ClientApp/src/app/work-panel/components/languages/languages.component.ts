import { Component, OnInit, Input } from "@angular/core";

import { SharePhraseService } from "../../localServices/share-phrase.service";
import { ShareTranslatedPhraseService } from "../../localServices/share-translated-phrase.service";
import { ShareWordFromModalService } from "../../localServices/share-word-from-modal.service";
import { TranslationService } from "../../../services/translationService.service";
import { ProjectsService } from "../../../services/projects.service";
import { AuthenticationService } from "src/app/services/authentication.service";

import { Translation } from "../../../models/database-entities/translation.type";
import { TranslationWithFile } from "../../localEntites/translations/translationWithFile.type";
import { SimilarTranslation } from "../../localEntites/translations/similarTranslation.type";
import { TranslationWithLocaleText } from "../../localEntites/translations/translationWithLocaleText.type";
import { Guid } from "guid-typescript";
// import 'jquery-ui/ui/widgets/tabs.js';
import * as $ from "jquery";

@Component({
  selector: "languages-component",
  templateUrl: "./languages.component.html",
  styleUrls: ["./languages.component.css"]
})
export class LanguagesComponent implements OnInit {
  listOfTranslations: Translation[];

  listOfTranslationsByMemory: TranslationWithFile[];

  listOfSimilarTranslations: SimilarTranslation[];

  listOfTranslationsInOtherLanguages: TranslationWithLocaleText[];

  newTranslation: Translation;

  searchByMemoryText: string = "";

  finalTranslationSelected: boolean;

  @Input() localeId: Guid;

  constructor(
    private sharePhraseService: SharePhraseService,
    private shareTranslatedPhraseService: ShareTranslatedPhraseService,
    private translationService: TranslationService,
    private projectService: ProjectsService,
    private shareWordFromModalService: ShareWordFromModalService,
    private authenticationService: AuthenticationService
  ) {
    this.listOfTranslations = [];
    this.listOfTranslationsByMemory = [];
    this.listOfSimilarTranslations = [];
    this.listOfTranslationsInOtherLanguages = [];

    // Событие, срабатываемое при выборе фразы для перевода
    this.sharePhraseService.onClick2.subscribe(translationsOfTheString => {
      this.listOfTranslations = translationsOfTheString;
      this.listOfTranslationsByMemory = [];
      this.listOfSimilarTranslations = [];
      this.listOfTranslationsInOtherLanguages = [];
      this.finalTranslationSelected = false;

      this.checkFinalTranslationSelected();
      this.findSimilarTranslations();
      this.findTranslationsInOtherLanguages();
    });

    // Событие, срабатываемое при выборе фразы для перевода
    this.sharePhraseService.onClick.subscribe(pickedPhrase => {
      this.searchByMemoryText = null;

      // переключает TabBar на вкладку "Предложения языка" при смене слова для перевода
      let activeTab = $(".languagesOptionsBlock .nav-tabs .active").attr(
        "href"
      );

      if (activeTab != "#nav-offers") {
        $("a[href='" + activeTab + "']")
          .removeClass("active show")
          .attr("aria-selected", false);
        $(activeTab).removeClass("active show");
      }
      $("a[href='#nav-offers']")
        .addClass("active show")
        .attr("aria-selected", true);
      $("#nav-offers").addClass("active show");
    });

    // Событие, срабатываемое при введении варианта перевода
    this.shareTranslatedPhraseService.onSumbit.subscribe(translation =>
      this.listOfTranslations.push(translation)
    );

    // Событие, срабатываемое при поиске слова по памяти переводов из модального окна
    this.shareWordFromModalService.onClickFindInMemory.subscribe(word => {
      this.searchByMemoryText = word;

      this.searchByMemory();

      // переключает TabBar на вкладку "Поиск по памяти переводов"
      let activeTab = $(".languagesOptionsBlock .nav-tabs .active").attr(
        "href"
      );

      if (activeTab != "#nav-memorySearch") {
        $("a[href='" + activeTab + "']")
          .removeClass("active show")
          .attr("aria-selected", false);
        $(activeTab).removeClass("active show");
      }
      $("a[href='#nav-memorySearch']")
        .addClass("active show")
        .attr("aria-selected", true);
      $("#nav-memorySearch").addClass("active show");
    });
  }

  ngOnInit(): void {}

  // Предоставляет доступ к финальному подтверждению перевода только пользователю с ролью "Менеджер" и "Владелец проекта"
  checkAccessToSelectTranslation(): boolean {
    let roles: string[] = ["Менеджер", "Владелец проекта"];

    if (this.authenticationService.provideAccessOnlyFor(roles)) {
      return true;
    } else {
      return false;
    }
  }

  // Проверяет наличие галочки в checkbox'е selected
  checkFinalTranslationSelected() {
    this.listOfTranslations.forEach(element => {
      if (element.selected) {
        this.finalTranslationSelected = true;
      }
    });
  }

  // Функция проверки наличия фразы
  checkPhrase(): boolean {
    if (this.sharePhraseService.getSharedPhrase() === undefined) {
      return false;
    } else {
      return true;
    }
  }

  // Функция проверки кол-ва вариантов перевода
  checkNumberOfTranslations() {
    if (this.listOfTranslations.length == 0) {
      return false;
    } else {
      return true;
    }
  }

  // Функция проверки кол-ва вариантов перевода по поиску по памяти
  checkNumberOfTranslationsByMemory() {
    if (this.listOfTranslationsByMemory.length == 0) {
      return false;
    } else {
      return true;
    }
  }

  // Функция проверки кол-ва схожих вариантов перевода
  checkNumberOfSimilarTranslations() {
    if (this.listOfSimilarTranslations.length == 0) {
      return false;
    } else {
      return true;
    }
  }

  checkNumberOfTranslationsInOtherLanguages() {
    if (this.listOfTranslationsInOtherLanguages.length == 0) {
      return false;
    } else {
      return true;
    }
  }

  // Изменяет статус фразы для перевода если меняется значение checkbox'а
  async changeStatusOfTranslationSubstring() {
    let status = "Empty";

    this.listOfTranslations.some(function(item) {
      if (item.confirmed == true) {
        status = "Confirmed";
      }
      if (item.selected == true) {
        status = "Selected";
      }
      return item.selected == true;
    });

    this.sharePhraseService.setStatusOfTranslationSubstring(status);
  }

  // Изменение подтвержедние перевода
  async changeTranslation(translation: Translation) {
    if (translation.confirmed == false) {
      this.translationService.rejectTranslate(translation.id);
    } else {
      this.translationService.acceptTranslate(translation.id);
    }
    this.changeStatusOfTranslationSubstring();
  }

  // Изменение выбранного финального перевода
  async changeSelectedTranslation(translation: Translation) {
    if (translation.selected == false) {
      this.finalTranslationSelected = false;
      translation.confirmed = false;
      this.translationService.rejectFinalTranslattion(translation.id);
    } else {
      this.finalTranslationSelected = true;
      translation.confirmed = true;
      this.translationService.acceptFinalTranslation(translation.id);
    }
    this.changeStatusOfTranslationSubstring();
  }

  // Удаление варианта перевода
  async deleteTranslateClick(translationId: Guid) {
    await this.translationService.deleteTranslation(translationId);
    for (var i = 0; i < this.listOfTranslations.length; i++) {
      if (this.listOfTranslations[i].id == translationId) {
        this.listOfTranslations.splice(i, 1);
        break;
      }
    }
  }

  // Поиск схожих вариантов перевода по нечеткому поиску
  findSimilarTranslations() {
    let phraseForTranslation = this.sharePhraseService.getSharedPhrase();
    let currentProjectId = this.projectService.currentProjectId;

    this.translationService
      .findSimilarTranslations(
        currentProjectId,
        phraseForTranslation,
        this.localeId
      )
      .subscribe(similarTranslations => {
        // Функция сортировки списка схожих вариантов перевода по убыванию
        function sortBySimilarValue(a, b) {
          if (a.similarity > b.similarity) return -1;
          if (a.similarity < b.similarity) return 1;
          return 0;
        }

        similarTranslations.sort(sortBySimilarValue);
        this.listOfSimilarTranslations = similarTranslations;
      });
  }

  findTranslationsInOtherLanguages() {
    let phraseForTranslation = this.sharePhraseService.getSharedPhrase();
    let currentProjectId = this.projectService.currentProjectId;

    this.translationService
      .findTranslationsInOtherLanguages(
        currentProjectId,
        phraseForTranslation,
        this.localeId
      )
      .subscribe(translationsInOtherLanguages => {
        // Функция сортировки списка схожих вариантов перевода по убыванию
        function sortBySimilarValue(a, b) {
          if (a.locale_name_text > b.locale_name_text) return -1;
          if (a.locale_name_text < b.locale_name_text) return 1;
          return 0;
        }

        translationsInOtherLanguages.sort(sortBySimilarValue);

        this.listOfTranslationsInOtherLanguages = translationsInOtherLanguages;
      });
  }

  // Событие, срабатываемое при нажатии клавиши Enter в поле "Поиск по памяти"
  onEnterPressByMemory(event: any) {
    if (event.which == 13 || event.keyCode == 13) {
      this.searchByMemory();
    }
  }

  // Поиск по памяти переводов
  searchByMemory() {
    let currentProjectId = this.projectService.currentProjectId;

    this.translationService
      .findTranslationByMemory(currentProjectId, this.searchByMemoryText)
      .subscribe(translations => {
        this.listOfTranslationsByMemory = translations;
      });

    this.searchByMemoryText = null;
  }
}
