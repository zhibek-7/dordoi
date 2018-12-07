import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { String } from 'src/app/models/database-entities/string.type';
import { ModalComponent } from 'src/app/glossaries/components/modal/modal.component';
import { PartOfSpeech } from 'src/app/models/database-entities/partOfSpeech.type';

@Component({
  selector: 'app-edit-term-form-modal',
  templateUrl: './edit-term-form-modal.component.html',
  styleUrls: ['./edit-term-form-modal.component.css']
})
export class EditTermFormComponent extends ModalComponent implements OnInit {

  @Input() partsOfSpeech: PartOfSpeech[];

  _originalTermReference: String;

  _term: String = new String();

  @Input()
  set term(value: String) {
    this._originalTermReference = value;
    this._term = (JSON.parse(JSON.stringify(value)));
  }

  @Output() termUpdateConfirmed = new EventEmitter<String>();

  constructor() { super(); }

  ngOnInit() {
  }

  confirmTermUpdate() {
    this.hide();
    this.termUpdateConfirmed.emit(this._term);
  }

  show() {
    super.show();
    this._term = (JSON.parse(JSON.stringify(this._originalTermReference)));
  }

}
