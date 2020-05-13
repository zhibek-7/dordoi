import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

import { TranslationMemoryService } from "src/app/services/translation-memory.service";
import { TranslationMemoryForEditingDTO } from "src/app/models/DTO/translationMemoryDTO.type";

@Component({
  selector: 'app-edit-translation-memory-form-modal',
  templateUrl: './edit-translation-memory-form-modal.component.html',
  styleUrls: ['./edit-translation-memory-form-modal.component.css']
})
export class EditTranslationMemoryFormModalComponent extends ModalComponent implements OnInit {
  @Input()
  translationMemory: TranslationMemoryForEditingDTO;

  translationMemoryEditable: TranslationMemoryForEditingDTO;

  @Output()
  editedSubmitted = new EventEmitter<TranslationMemoryForEditingDTO>();

  @Output()
  error = new EventEmitter();

  constructor(private translationMemoryService: TranslationMemoryService) {
    super();
    this.translationMemoryEditable = new TranslationMemoryForEditingDTO();
  }

  ngOnInit() {
  }

  async show() {
    await this.loadForEditing();
    if (this.translationMemoryEditable)
      super.show();
    else {
      this.hide();
      alert("Не удалось открыть для редактирования память переводов \"" + this.translationMemory.name_text + "\", ранее он был удален. \n" +
        "Страница будет перезагружена для обновления данных.");
      this.translationMemoryEditable = new TranslationMemoryForEditingDTO();
      this.error.emit();
    }
  }

  async loadForEditing() {
    this.translationMemoryEditable = await this.translationMemoryService.getForEditing(this.translationMemory.id);
  }

  submit() {
    this.hide();
    this.editedSubmitted.emit(this.translationMemoryEditable);
  }

}
