import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { String } from 'src/app/models/database-entities/string.type';
import { ModalComponent } from 'src/app/glossaries/components/modal/modal.component';

import { PartsOfSpeechService } from 'src/app/services/partsOfSpeech.service';
import { PartOfSpeech } from 'src/app/models/database-entities/partOfSpeech.type';
import { Glossary } from 'src/app/models/database-entities/glossary.type';

@Component({
  selector: 'app-add-term-form-modal',
  templateUrl: './add-term-form-modal.component.html',
  styleUrls: ['./add-term-form-modal.component.css']
})
export class AddTermFormComponent extends ModalComponent implements OnInit {

  newTerm = new String();

  partsOfSpeech: PartOfSpeech[];

  @Output() newTermSubmitted = new EventEmitter<String>();

  @Input()
  set glossary(value: Glossary) {
    if (!value)
      return;

    this.partsOfSpeechService.getListByGlossaryId(value.id)
      .subscribe(
        partsOfSpeech => this.partsOfSpeech = partsOfSpeech,
        error => console.log(error)
      );
  }

  constructor(private partsOfSpeechService: PartsOfSpeechService) { super(); }

  ngOnInit() {
  }

  submitNewTerm() {
    this.hide();
    this.newTermSubmitted.emit(this.newTerm);
    this.newTerm = new String();
  }

}
