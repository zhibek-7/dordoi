import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { String } from 'src/app/models/database-entities/string.type';
import { ModalComponent } from 'src/app/glossaries/components/modal/modal.component';
import { PartOfSpeech } from 'src/app/models/database-entities/partOfSpeech.type';

@Component({
  selector: 'app-add-term-form-modal',
  templateUrl: './add-term-form-modal.component.html',
  styleUrls: ['./add-term-form-modal.component.css']
})
export class AddTermFormComponent extends ModalComponent implements OnInit {

  newTerm = new String();

  @Input() partsOfSpeech: PartOfSpeech[];

  @Output() newTermSubmitted = new EventEmitter<String>();

  constructor() { super(); }

  ngOnInit() {
  }

  submitNewTerm() {
    this.newTerm.substringToTranslate = this.newTerm.value;
    this.newTerm.context = 'newContext';
    this.newTerm.positionInText = 0;
    this.hide();
    this.newTermSubmitted.emit(this.newTerm);
    this.newTerm = new String();
  }

}
