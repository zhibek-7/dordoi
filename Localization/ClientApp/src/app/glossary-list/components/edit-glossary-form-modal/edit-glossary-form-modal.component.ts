import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

import { GlossariesForEditing } from 'src/app/models/DTO/glossariesDTO.type';
import { Glossary } from 'src/app/models/database-entities/glossary.type';
import { GlossaryService } from 'src/app/services/glossary.service';

@Component({
  selector: 'app-edit-glossary-form-modal',
  templateUrl: './edit-glossary-form-modal.component.html',
  styleUrls: ['./edit-glossary-form-modal.component.css']
})
export class EditGlossaryFormModalComponent extends ModalComponent implements OnInit {
  @Input()
  glossary: Glossary;

  glossaryEditable: GlossariesForEditing;

  @Output()
  editedGlossarySubmitted = new EventEmitter<GlossariesForEditing>();

  @Output()
  error = new EventEmitter();

  constructor(private glossariesService: GlossaryService) {
    super();
    this.glossaryEditable = new GlossariesForEditing();
  }

  ngOnInit() {
  }

  async show() {
    await this.loadGlossaryForEditing();
    if (this.glossaryEditable)
      super.show();
    else {
      this.hide();
      alert("Не удалось открыть для редактирования глоссарий \"" + this.glossary.name_text + "\", ранее он был удален. \n" +
        "Страница будет перезагружена для обновления данных.");
      this.glossaryEditable = new GlossariesForEditing();
      this.error.emit();
    }
  }

  async loadGlossaryForEditing() {
    this.glossaryEditable = await this.glossariesService.getGlossaryForEditing(this.glossary.id);
  }

  submit() {
    this.hide();
    this.editedGlossarySubmitted.emit(this.glossaryEditable);
  }

}
