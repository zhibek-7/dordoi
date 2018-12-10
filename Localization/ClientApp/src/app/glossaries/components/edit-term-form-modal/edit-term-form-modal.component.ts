import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { String } from 'src/app/models/database-entities/string.type';
import { ModalComponent } from 'src/app/glossaries/components/modal/modal.component';
import { PartOfSpeech } from 'src/app/models/database-entities/partOfSpeech.type';
import { Term } from 'src/app/models/Glossaries/term.type';
import { GlossariesService } from 'src/app/services/glossaries.service';
import { Glossary } from 'src/app/models/database-entities/glossary.type';

@Component({
  selector: 'app-edit-term-form-modal',
  templateUrl: './edit-term-form-modal.component.html',
  styleUrls: ['./edit-term-form-modal.component.css']
})
export class EditTermFormComponent extends ModalComponent implements OnInit {

  @Input() glossary: Glossary;

  @Input() partsOfSpeech: PartOfSpeech[];

  _originalTermReference: String;

  _term: Term = new Term();

  @Input()
  set term(value: String) {
    this._originalTermReference = value;
  }

  @Output() termUpdateConfirmed = new EventEmitter<Term>();

  constructor(private glossariesService: GlossariesService) { super(); }

  ngOnInit() {
  }

  confirmTermUpdate() {
    this.hide();
    this.termUpdateConfirmed.emit(this._term);
  }

  show() {
    super.show();
    this.resetTermViewModel();
  }

  resetTermViewModel() {
    this.glossariesService.getTermPartOfSpeechId(this.glossary.id, this._originalTermReference.id)
      .subscribe(partOfSpeechId => {
          let termStringClone = (JSON.parse(JSON.stringify(this._originalTermReference)));
          this._term = new Term(
            termStringClone.id,
            termStringClone.substringToTranslate,
            termStringClone.description,
            termStringClone.context,
            termStringClone.id_fileOwner,
            termStringClone.translationMaxLength,
            termStringClone.value,
            termStringClone.positionInText,
            partOfSpeechId);
          if (!this._term.partOfSpeechId) {
            this._term.partOfSpeechId = null;
          }
        },
        error => console.log(error));
  }

}
