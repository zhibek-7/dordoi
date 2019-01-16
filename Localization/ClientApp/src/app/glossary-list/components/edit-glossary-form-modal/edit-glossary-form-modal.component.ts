import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

import { GlossariesForEditing } from 'src/app/models/DTO/glossariesDTO.type';
import { Locale } from 'src/app/models/database-entities/locale.type';
import { localizationProjectForSelectDTO } from 'src/app/models/DTO/localizationProjectForSelectDTO.type';

@Component({
  selector: 'app-edit-glossary-form-modal',
  templateUrl: './edit-glossary-form-modal.component.html',
  styleUrls: ['./edit-glossary-form-modal.component.css']
})
export class EditGlossaryFormModalComponent extends ModalComponent  implements OnInit
{
  @Input()
  newGlossary: GlossariesForEditing; //переименовать в glossary

  @Output()
  editedGlossarySubmitted = new EventEmitter<GlossariesForEditing>();

  constructor()
  {
    super();
  }

  ngOnInit()
  {
  }

  show()
  {
    super.show();
  }

  submit() {
    this.hide();
    //
    console.log(this.newGlossary.name)
    console.log(this.newGlossary.locales.length)
    console.log(this.newGlossary.localizationProjects.length)
    //
    this.editedGlossarySubmitted.emit(this.newGlossary);
  }

}
