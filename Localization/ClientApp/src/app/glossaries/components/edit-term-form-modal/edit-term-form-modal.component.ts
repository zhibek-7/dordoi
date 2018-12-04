import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { String } from 'src/app/models/database-entities/string.type';
import { PartsOfSpeechService } from 'src/app/services/partsOfSpeech.service';
import { ModalComponent } from 'src/app/glossaries/components/modal/modal.component';
import { PartOfSpeech } from 'src/app/models/database-entities/partOfSpeech.type';

@Component({
  selector: 'app-edit-term-form-modal',
  templateUrl: './edit-term-form-modal.component.html',
  styleUrls: ['./edit-term-form-modal.component.css']
})
export class EditTermFormComponent extends ModalComponent implements OnInit {

  partsOfSpeech: PartOfSpeech[];

  _originalTermReference: String;

  _term: String = new String();

  @Input()
  set term(value: String) {
    this._originalTermReference = value;
    this._term = (JSON.parse(JSON.stringify(value)));
  }

  @Output() termUpdateConfirmed = new EventEmitter<String>();

  constructor(private partsOfSpeechService: PartsOfSpeechService) { super(); }

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
