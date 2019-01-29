import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { ShowImageModalComponent } from '../show-image-modal/show-image-modal';

import { TranslationSubstringService } from 'src/app/services/translationSubstring.service';

import { TranslationSubstring } from 'src/app/models/database-entities/translationSubstring.type';
import { Image } from 'src/app/models/database-entities/image.type';

@Component({
    selector: 'context-edit-modal-component',
    templateUrl: './context-edit-modal.component.html',
    styleUrls: ['./context-edit-modal.component.css']
})
export class ContextEditModalComponent extends ModalComponent implements OnInit {

    enteredContext: string = null;
    translationMaxLength: number = 0;

    filesToUpload: File[];
    images: Image[];

    @Input() currentPhrase: TranslationSubstring;

    @Output() onEnterContext = new EventEmitter<TranslationSubstring>();
    
    constructor(
        private translationSubstringService: TranslationSubstringService,
        private showImageDialog: MatDialog) {    

        super();

        this.filesToUpload = [];
        this.images = [];
     }

    ngOnInit(): void {

    }

    // Событие при нажатии кнопки "Сохранить"
    confirmButtunClick(){
        this.currentPhrase.context = this.enteredContext;
        this.currentPhrase.translationMaxLength = this.translationMaxLength;

        if (this.filesToUpload != null) {
            this.loadScrinshot();
          }

        this.onEnterContext.emit(this.currentPhrase);
        super.hide();
    }

    // Функция загрузки скриншота
    loadScrinshot() {
        this.translationSubstringService.uploadImageToTranslationSubstring(this.filesToUpload, this.currentPhrase.id);
    }

    // Открытие модального окна
    show(){
        super.show();
                
        this.translationMaxLength = this.currentPhrase.translationMaxLength;
        this.enteredContext = this.currentPhrase.context;

        this.loadImages(this.currentPhrase.id);
    }

    // Функция контролирующая кол-во введенных символов
    countNumberOfSymbols(){
        if(this.translationMaxLength < 0 || this.translationMaxLength > 9999){
            this.translationMaxLength = 0;
        }        
    }

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

        super.hide();
        let dialogRef = this.showImageDialog.open(ShowImageModalComponent, dialogConfig);
    }

    //Функция, срабатываемая при загрузке скриншота
    handleFileInput(file: FileList) {
        this.filesToUpload.push(file.item(0));
        if(this.images == undefined){
          this.images = [];
        }
  
        var reader = new FileReader();
        reader.onload = (event: any) => {
  
          var insertedImage = new Image();
          insertedImage.data = event.target.result;
  
          //обрезаем дополнительную информацию о изображении и оставляем только byte[]
          insertedImage.data = insertedImage.data.match(".*base64,(.*)")[1];

          this.images.push(insertedImage)        
        }
        reader.readAsDataURL(this.filesToUpload[this.filesToUpload.length - 1]);
      }

}
