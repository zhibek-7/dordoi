import { Component, OnInit, Output, EventEmitter, OnDestroy, Input } from "@angular/core";
import { MatDialog, MatDialogConfig } from "@angular/material";

import { ShowImageModalComponent } from "../show-image-modal/show-image-modal";
import { SelectedWordModalComponent } from "../selected-word-modal/selected-word-modal.component";

import { SharePhraseService } from "../../localServices/share-phrase.service";
import { ShareTranslatedPhraseService } from "../../localServices/share-translated-phrase.service";
import { TranslationService } from "../../../services/translationService.service";
import { TranslationSubstringService } from "src/app/services/translationSubstring.service";

import { TranslationSubstring } from "../../../models/database-entities/translationSubstring.type";
import { Translation } from "../../../models/database-entities/translation.type";
import { Image } from "src/app/models/database-entities/image.type";
import { UserService } from "src/app/services/user.service";
import { Guid } from 'guid-typescript';
declare var $: any;

@Component({
  selector: "translation-component",
  templateUrl: "./translation.component.html",
  styleUrls: ["./translation.component.css"],
  providers: [TranslationSubstringService, UserService]
})
export class TranslationComponent implements OnInit, OnDestroy {
  showContextBlock: boolean = true;
  showLeftBlock: boolean = true;
  showRightBlock: boolean = true;
  @Output() onChangedLeftBlock = new EventEmitter<boolean>();
  @Output() onChangedRightBlock = new EventEmitter<boolean>();

  @Input() localeId: Guid;

  translatedText: string;
  phraseForTranslate: TranslationSubstring;
  translatedPhrase: Translation;

  images: Image[];

  changingTranslation: boolean = false;

  constructor(
    private sharePhraseService: SharePhraseService,
    private shareTranslatedPhraseService: ShareTranslatedPhraseService,
    private translationService: TranslationService,
    private translationSubstringService: TranslationSubstringService,
    private selectionDialog: MatDialog,
    private showImageDialog: MatDialog,
  ) {
    this.images = [];

    // Событие, срабатываемое при выборе фразы для перевода
    this.sharePhraseService.onClick.subscribe(pickedPhrase => {
      this.phraseForTranslate = pickedPhrase;
      this.translatedText = null;      

      this.loadImages(pickedPhrase.id);
      this.cancelChangeingTranslation();
    });

    // Событие, происходящие при клике на фразу из списков предложенных вариантов(для каждой из всех трех вкладок)
    this.shareTranslatedPhraseService.onClickTranslation.subscribe(pickedTranslation => {    
      this.cancelChangeingTranslation();
    
      this.translatedText = pickedTranslation.translation_variant;
    });

    // Событие, происходящие при клике на уже существующий вариант перевода
    this.shareTranslatedPhraseService.onClickCurrentTranslation.subscribe(pickedTranslation => {
      this.translatedText = pickedTranslation.translated;
      this.translatedPhrase = pickedTranslation;

      this.changingTranslation = true;
    })

  }

  //Действия при двойном клике по слову
  openSelectionDialog(event) {
    let selectedWord;
    if (window.getSelection) {
      selectedWord = window.getSelection();
    } else if (document.getSelection) {
      selectedWord = document.getSelection();
    }

    const dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      selectedWord: selectedWord.toString()
    };
    dialogConfig.panelClass = "selectionDialog";

    let positionY: string = event.clientY + "px";
    let positionX: string = event.clientX + "px";
    dialogConfig.position = {
      top: positionY,
      left: positionX
    };

    //TODO не используется let dialogRef = 
    this.selectionDialog.open(
      SelectedWordModalComponent,
      dialogConfig
    );
  }

  ngOnInit(): void {}

  ngOnDestroy(): void {
    this.sharePhraseService.setSharedPhraseToNull();
  }

  // Скрывает левый блок (Блок с фразами)
  hideLeftBlock() {
    if (this.showLeftBlock) {
      this.showLeftBlock = false;
    } else {
      this.showLeftBlock = true;
    }
    this.onChangedLeftBlock.emit(this.showLeftBlock);
  }

  // Скрывает правый блок (Блок с комментариями)
  hideRightBlock() {
    if (this.showRightBlock) {
      this.showRightBlock = false;
    } else {
      this.showRightBlock = true;
    }
    this.onChangedRightBlock.emit(this.showRightBlock);
  }

  // Показать / Скрыть контекст
  contextShowClick() {
    if (this.showContextBlock) {
      this.showContextBlock = false;
    } else {
      this.showContextBlock = true;
    }
  }

  arrowLeftClick() {
    alert("Действие при нажатии левой стрелки");
  }

  arrowRightClick() {
    alert("Действие при нажатии правой стрелки");
  }

  importFileClick() {
    alert("Импортируем файл");
  }

  // Событие, срабатывающее при сохранении изменений в модальном окне
  enterContext(changedTranslationSubstring: TranslationSubstring) {
    this.phraseForTranslate.context = changedTranslationSubstring.context;
    this.phraseForTranslate.context_file = changedTranslationSubstring.context_file;
    this.phraseForTranslate.translation_max_length =
      changedTranslationSubstring.translation_max_length;

    this.translationSubstringService
      .updateTranslationSubstring(this.phraseForTranslate)
      .subscribe( success => {
        this.loadImages(this.phraseForTranslate.id);
      });
  }

  // Сохранить вариант перевода
  async submitTranslate() {    

    this.translatedPhrase = new Translation(
      this.translatedText,
      this.phraseForTranslate.id,
      this.localeId
    );
    let insertedTranslationId = await this.translationService.createTranslation(
      this.translatedPhrase
    );
    this.translatedPhrase.id = insertedTranslationId;

    await this.translationService.getAllTranslationsInStringById(
      this.phraseForTranslate.id
    );
    this.shareTranslatedPhraseService.sumbitTranslatedPhrase(
      this.translatedPhrase
    );

    this.translatedText = null;
    this.translatedPhrase = null;

    $("#btnSave").attr("disabled", true);
    $("#btnSave").attr("disabled", false); // хорошо бы найти стиль который убирает обводку кнопки после нажатия(убирать его другим способом)
  }

  // Проверка наличия выбранной фразы
  checkPhrase(): boolean {
    if (this.sharePhraseService.getSharedPhrase() === undefined) {
      return false;
    } else {
      return true;
    }
  }  

  loadImages(translationSubstringId: Guid) {
    this.translationSubstringService
      .getImagesByTranslationSubstringId(translationSubstringId)
      .subscribe(images => {
        this.images = images;
      });
  }

  // Функция отображения скриншота в модальном окне в увеличенном размере
  showImage(image: Image) {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      selectedImage: image
    };

    //TODO не используется let dialogRef = 
    this.showImageDialog.open(
      ShowImageModalComponent,
      dialogConfig
    );
  }

  changeTranslation(){
    this.translatedPhrase.translated = this.translatedText;    

    this.translationService.updateTranslation(this.translatedPhrase)
                .subscribe();

    this.cancelChangeingTranslation();
  }

  cancelChangeingTranslation(){
    this.translatedText = null;
    this.translatedPhrase = null;
    this.changingTranslation = false;
  }
}
