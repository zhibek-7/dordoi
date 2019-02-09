import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { MatDialog, MatDialogConfig } from "@angular/material";

import { ShowImageModalComponent } from '../show-image-modal/show-image-modal';
import { SelectedWordModalComponent } from '../selected-word-modal/selected-word-modal.component';

import { SharePhraseService } from "../../localServices/share-phrase.service";
import { ShareTranslatedPhraseService } from "../../localServices/share-translated-phrase.service";
import { TranslationService } from "../../../services/translationService.service";
import { TranslationSubstringService } from "src/app/services/translationSubstring.service";

import { TranslationSubstring } from '../../../models/database-entities/translationSubstring.type';
import { Translation } from '../../../models/database-entities/translation.type';
import { Image } from 'src/app/models/database-entities/image.type';

declare var $: any;

@Component({
  selector: "translation-component",
  templateUrl: "./translation.component.html",
  styleUrls: ["./translation.component.css"],
  providers: [TranslationSubstringService]
})
export class TranslationComponent implements OnInit {
  showContextBlock: boolean = true;
  showLeftBlock: boolean = true;
  showRightBlock: boolean = true;
  @Output() onChangedLeftBlock = new EventEmitter<boolean>();
  @Output() onChangedRightBlock = new EventEmitter<boolean>();

  translatedText: string;
  phraseForTranslate: TranslationSubstring;
  translatedPhrase: Translation;

  constructor(
    private sharePhraseService: SharePhraseService,
    private shareTranslatedPhraseService: ShareTranslatedPhraseService,
    private translationService: TranslationService,
    private translationSubstringService: TranslationSubstringService,
    private selectionDialog: MatDialog
  ) {
    // Событие, срабатываемое при выборе фразы для перевода
    this.sharePhraseService.onClick.subscribe(pickedPhrase => {
      this.phraseForTranslate = pickedPhrase;
      this.translatedText = null;
    });
  }

    images: Image[];

    constructor(private sharePhraseService: SharePhraseService,
        private shareTranslatedPhraseService: ShareTranslatedPhraseService, 
        private translationService: TranslationService,
        private translationSubstringService: TranslationSubstringService,
        private selectionDialog: MatDialog,
        private showImageDialog: MatDialog) {

        this.images = [];

        // Событие, срабатываемое при выборе фразы для перевода
        this.sharePhraseService.onClick.subscribe(pickedPhrase => {
                this.phraseForTranslate = pickedPhrase;                
                this.translatedText = null;

                this.loadImages(pickedPhrase.id);
            });                              
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

    let dialogRef = this.selectionDialog.open(
      SelectedWordModalComponent,
      dialogConfig
    );
  }

  ngOnInit(): void {}

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

  thisIsAtest() {
    alert("magic");
  }

  // Событие, срабатывающее при сохранении изменений в модальном окне
  enterContext(changedTranslationSubstring: TranslationSubstring) {
    this.phraseForTranslate.context = changedTranslationSubstring.context;
    this.phraseForTranslate.translation_max_length =
      changedTranslationSubstring.translation_max_length;

    this.translationSubstringService
      .updateTranslationSubstring(this.phraseForTranslate)
      .subscribe();
  }

  // Сохранить вариант перевода
  async submitTranslate() {
    this.translatedPhrase = new Translation(
      this.translatedText,
      this.phraseForTranslate.id,
      10123,
      300
    ); //поменять потом на реальный id пользователя и id языка
    let insertedTranslationId = await this.translationService.createTranslate(
      this.translatedPhrase
    );
    this.translatedPhrase.id = insertedTranslationId;

    loadImages( translationSubstringId: number) {
        this.translationSubstringService.getImagesByTranslationSubstringId(translationSubstringId)
            .subscribe(
                images => {
                    this.images = images;
                }
            );
    }

    // Функция отображения скриншота в модальном окне в увеличенном размере
    showImage(image: Image){
        const dialogConfig = new MatDialogConfig();

        dialogConfig.data = {
            selectedImage: image
        };

        let dialogRef = this.showImageDialog.open(ShowImageModalComponent, dialogConfig);
    }

}
