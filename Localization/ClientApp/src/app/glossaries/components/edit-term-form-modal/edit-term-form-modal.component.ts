import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { String } from 'src/app/models/database-entities/string.type';
import { ModalComponent } from 'src/app/glossaries/components/modal/modal.component';
import { PartOfSpeech } from 'src/app/models/database-entities/partOfSpeech.type';
import { Term } from 'src/app/models/Glossaries/term.type';
import { GlossariesService } from 'src/app/services/glossaries.service';
import { Glossary } from 'src/app/models/database-entities/glossary.type';
import { Locale } from 'src/app/models/database-entities/locale.type';
import { PartsOfSpeechService } from 'src/app/services/partsOfSpeech.service';

@Component({
  selector: 'app-edit-term-form-modal',
  templateUrl: './edit-term-form-modal.component.html',
  styleUrls: ['./edit-term-form-modal.component.css']
})
export class EditTermFormComponent extends ModalComponent implements OnInit {

  @Input() glossary: Glossary;

  _originalTermReference: Term;

  _term: Term = new Term();

  @Input()
  set term(value: Term) {
    this._originalTermReference = value;
  }

  @Output() termUpdateConfirmed = new EventEmitter<Term>();

  constructor() { super(); }

  ngOnInit() {
  }

  confirmTermUpdate() {
    this.hide();
    this.termUpdateConfirmed.emit(this._term);
  }

  show() {
    this.resetTermViewModel();
    super.show();
  }

  resetTermViewModel() {
    this._term = (JSON.parse(JSON.stringify(this._originalTermReference)));
    if (!this._term.partOfSpeechId) {
      this._term.partOfSpeechId = null;
    }
  }

}
