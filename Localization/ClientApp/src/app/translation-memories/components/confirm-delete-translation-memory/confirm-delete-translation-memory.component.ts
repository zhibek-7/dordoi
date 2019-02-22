import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

import { TranslationMemoryForEditingDTO } from 'src/app/models/DTO/translationMemoryDTO.type';

@Component({
  selector: 'app-confirm-delete-translation-memory',
  templateUrl: './confirm-delete-translation-memory.component.html',
  styleUrls: ['./confirm-delete-translation-memory.component.css']
})
export class ConfirmDeleteTranslationMemoryComponent extends ModalComponent implements OnInit {
  @Input()
  translationMemory: TranslationMemoryForEditingDTO;

  @Output()
  confirmedDelete = new EventEmitter<TranslationMemoryForEditingDTO>();

  constructor() { super(); }

  ngOnInit() {
  }

  delete() {
    this.hide();
    this.confirmedDelete.emit(this.translationMemory);
  }

}
