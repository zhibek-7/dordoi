import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

import { TranslationSubstring } from 'src/app/models/database-entities/translationSubstring.type';
// import { NullTemplateVisitor } from '@angular/compiler';

@Component({
    selector: 'context-edit-modal-component',
    templateUrl: './context-edit-modal.component.html',
    styleUrls: ['./context-edit-modal.component.css']
})
export class ContextEditModalComponent extends ModalComponent implements OnInit {

    enteredContext: string = null;
    translationMaxLength: number = 0;

    filesToUpload: File[];

    @Input() currentPhrase: TranslationSubstring;

    @Output() onEnterContext = new EventEmitter<TranslationSubstring>();
    
    constructor() {    
        super();

        this.filesToUpload = [];
     }

    ngOnInit(): void {

    }

    // Событие при нажатии кнопки "Сохранить"
    confirmButtunClick(){
        this.currentPhrase.context = this.enteredContext;
        this.currentPhrase.translationMaxLength = this.translationMaxLength;

        this.onEnterContext.emit(this.currentPhrase);
        super.hide();
    }

    // Открытие модального окна
    show(){
        super.show();
        
        this.translationMaxLength = this.currentPhrase.translationMaxLength;
        this.enteredContext = this.currentPhrase.context;

        console.log(this.currentPhrase);
    }

    // Функция контролирующая кол-во введенных символов
    countNumberOfSymbols(){
        if(this.translationMaxLength < 0 || this.translationMaxLength > 9999){
            this.translationMaxLength = 0;
        }        
    }

    // Функция, срабатываемая при загрузке скриншота
    // handleFileInput(file: FileList) {
    //     this.filesToUpload.push(file.item(0));
    //     // if(this.changedComment.images == undefined){
    //     //   this.changedComment.images = [];
    //     // }
  
    //     var reader = new FileReader();
    //     reader.onload = (event: any) => {
  
    //       var insertedImage = new Image();
    //     //   insertedImage.data = event.target.result;
  
    //       this.changedComment.images.push(insertedImage)        
    //     }
    //     reader.readAsDataURL(this.filesToUpload[this.filesToUpload.length - 1]);
    //   }

}
