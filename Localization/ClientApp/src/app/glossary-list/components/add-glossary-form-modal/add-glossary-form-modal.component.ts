import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

import { Glossaries } from 'src/app/models/DTO/glossaries.type';
import { Locale } from 'src/app/models/database-entities/locale.type';
import { localizationProjectForSelectDTO } from 'src/app/models/DTO/localizationProjectForSelectDTO.type';

@Component({
  selector: 'app-add-glossary-form-modal',
  templateUrl: './add-glossary-form-modal.component.html',
  styleUrls: ['./add-glossary-form-modal.component.css']
})
export class AddGlossaryFormModalComponent extends ModalComponent  implements OnInit
{
  newGlossary: Glossaries;

  @Output()
  newGlossarySubmitted = new EventEmitter<Glossaries>();

  constructor()
  {
    super();
    this.resetNewGlossaryModel();
  }

  ngOnInit()
  {
  }

  show()
  {
    super.show();
    this.resetNewGlossaryModel();
  }

  submitNewGlossary() {
    this.hide();
    this.newGlossarySubmitted.emit(this.newGlossary);
  }

  resetNewGlossaryModel() {
    this.newGlossary = new Glossaries();
    this.newGlossary.locales = [];
    this.newGlossary.localizationProjects = [];
  }

}
