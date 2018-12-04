import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { String } from 'src/app/models/database-entities/string.type';
import { ModalComponent } from 'src/app/glossaries/components/modal/modal.component';
import { PartsOfSpeechService } from 'src/app/services/partsOfSpeech.service';

@Component({
  selector: 'app-add-term-form-modal',
  templateUrl: './add-term-form-modal.component.html',
  styleUrls: ['./add-term-form-modal.component.css']
})
export class AddTermFormComponent extends ModalComponent implements OnInit {

  newTerm = new String();

  @Output() newTermSubmitted = new EventEmitter<String>();

  constructor(private partsOfSpeechService: PartsOfSpeechService) { super(); }

  ngOnInit() {
  }

  submitNewTerm() {
    this.hide();
    this.newTermSubmitted.emit(this.newTerm);
    this.newTerm = new String();
  }

}
