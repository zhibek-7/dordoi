import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

import { TranslationMemoryForEditingDTO } from 'src/app/models/DTO/translationMemoryDTO.type';

@Component({
  selector: 'app-confirm-clear-translation-memory-of-string',
  templateUrl: './confirm-clear-translation-memory-of-string.component.html',
  styleUrls: ['./confirm-clear-translation-memory-of-string.component.css']
})
export class ConfirmClearTranslationMemoryOfStringComponent extends ModalComponent implements OnInit {
  @Input()
  translationMemory: TranslationMemoryForEditingDTO;

  @Output()
  confirmedClear = new EventEmitter<TranslationMemoryForEditingDTO>();

  constructor() { super(); }

  ngOnInit() {
  }

  clear() {
    this.hide();
    this.confirmedClear.emit(this.translationMemory);
  }

}
