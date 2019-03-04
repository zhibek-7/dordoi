import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

import { TranslationSubstringForEditingDTO } from "../../../models/DTO/TranslationSubstringDTO.type";

@Component({
  selector: 'app-confirm-delete-string-project-form-modal',
  templateUrl: './confirm-delete-string-project-form-modal.component.html',
  styleUrls: ['./confirm-delete-string-project-form-modal.component.css']
})
export class ConfirmDeleteStringProjectFormModalComponent extends ModalComponent implements OnInit {

  @Input()
  translationSubstrings: TranslationSubstringForEditingDTO[];

  @Output()
  confirmedDelete = new EventEmitter<boolean>();
  
  translationSubstringsSubstringToTranslate: string;

  constructor() {
    super();
  }

  ngOnInit() { }

  show() {
    this.translationSubstringsSubstringToTranslate = this.translationSubstrings.map(t => t.substring_to_translate).join(', ');
    super.show();
  }

  delete() {
    this.hide();
    this.confirmedDelete.emit(true);
  }

}
